using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using Train.Enum;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Train
{
    public class UserCourseRecordDetailAppService : FRMSCoreAppServiceBase, IUserCourseRecordDetailAppService
    {
        private readonly IRepository<UserCourseRecordDetail, Guid> _repository;
        private readonly IRepository<Course, Guid> _couserRepository;
        private readonly IRepository<UserCourseRecord, Guid> _courseRecordRepository;
        private readonly ICourseSettingAppService _courseSettingAppService;
        private readonly IUserTrainScoreRecordAppService _trainScoreRecordAppService;
        private readonly IRepository<UserTrainScoreRecord, Guid> _trainScoreRecordRepository;
        public UserCourseRecordDetailAppService(IRepository<UserCourseRecordDetail, Guid> repository
            , IRepository<Course, Guid> couserRepository, ICourseSettingAppService courseSettingAppService
            , IRepository<UserCourseRecord, Guid> courseRecordRepository, IUserTrainScoreRecordAppService trainScoreRecordAppService
            , IRepository<UserTrainScoreRecord, Guid> trainScoreRecordRepository

        )
        {
            this._repository = repository;
            _couserRepository = couserRepository;
            _courseSettingAppService = courseSettingAppService;
            _courseRecordRepository = courseRecordRepository;
            _trainScoreRecordAppService = trainScoreRecordAppService;
            _trainScoreRecordRepository = trainScoreRecordRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<UserCourseRecordDetailListOutputDto>> GetList(GetUserCourseRecordDetailListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new UserCourseRecordDetailListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            CourseId = a.CourseId,
                            LearningTime = a.LearningTime,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<UserCourseRecordDetailListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<UserCourseRecordDetailOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<UserCourseRecordDetailOutputDto>();
		}

        /// <summary>
        /// 添加一个UserCourseRecordDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateUserCourseRecordDetailInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var chkCreate = false;
            //查询出课程
            var couser = await _couserRepository.FirstOrDefaultAsync(x => x.Id == input.CourseId && x.Status == -1);
            if (couser == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "课程不存在！");
            }
            //判断修习时长是否超过课程时长
            if (input.LearningTime > couser.LearnTime)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "修习时长不能超过课程总时长！");
            }
            //查询出上一条观看记录
            var model = await _repository.GetAll().Where(x => x.UserId == user.Id && x.CourseId == input.CourseId)
                .OrderByDescending(x => x.LastModificationTime ?? x.CreationTime).FirstOrDefaultAsync();
            if (model == null)
            {
                //判断第一次修习时长是否大于1分钟
                if (input.LearningTime > 1)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "初次记录修习时长不得高于1分钟！");
                }
                chkCreate = true;
            }
            else
            {
                //判断当前时间-上一条观看时间是否小于1分钟
                if (DateTime.Now - (model.LastModificationTime ?? model.CreationTime) < TimeSpan.FromSeconds(50))
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请勿短时间内多次上传课时！");
                }
                //判断本次上传修习时长-上一条观看记录修习时长是否大于1分钟
                if (input.LearningTime - model.LearningTime > 1)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "单次记录修习时长不得高于1分钟！");
                }
                //判断当前时间与上一条观看时间是否在同一天
                if (!DateTime.Now.Date.Equals(model.CreationTime.Date))
                {
                    chkCreate = true;
                }
            }
            if (chkCreate)
            {
                //创建新的修习记录
                model = new UserCourseRecordDetail()
                {
                    CourseId = input.CourseId,
                    LearningTime = input.LearningTime,
                    UserId = user.Id
                };
                await _repository.InsertAsync(model);
            }
            else
            {
                //更新修习时长
                model.LearningTime = input.LearningTime;
                await _repository.UpdateAsync(model);
            }
            var record = await _courseRecordRepository.GetAll()
                .Where(x => !x.IsDeleted && x.CourseId == model.CourseId && x.UserId == model.UserId)
                .FirstOrDefaultAsync();
            if (record != null)
            {
                //更新总时长
                record.LearnTime = model.LearningTime;
                //判断课程的修习状态进行完成操作
                if (record.IsComplete == null)
                {
                    var set = await _courseSettingAppService.Get();
                    //判断当前观看比率是否大于set的观看比率
                    if (Convert.ToDecimal(model.LearningTime) / Convert.ToDecimal(couser.LearnTime) * 100 >=
                        set.ViewingRatio)
                    {
                        //无计时或尚未超出规定学习时间
                        if (couser.ComplateTime == null || couser.ComplateTime > DateTime.Now)
                        {
                            //查询set并送出积分
                            var setScore = await _courseSettingAppService.GetSetVal(couser.LearnType, couser.IsSpecial);
                            if (setScore.ClassHourScore > 0)
                            {
                                await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                                {
                                    FromType = TrainScoreFromType.CourseLearn,
                                    FromId = record.CourseId,
                                    Score = setScore.ClassHourScore,
                                    UserId = user.Id
                                });
                            }
                            //更新课程观看状态
                            record.IsComplete = true;
                        }
                        else
                        {
                            //有计时并且超时当时完成的需要恢复之前扣除的积分
                            //恢复积分
                            var oldscore =
                                await _trainScoreRecordRepository.FirstOrDefaultAsync(x => x.BusinessId == record.Id);
                            if (oldscore != null)
                            {
                                await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                                {
                                    FromId = model.Id,
                                    FromType = TrainScoreFromType.CourseLearn,
                                    Score = oldscore.Score,
                                    UserId = user.Id
                                });
                            }
                        }
                    }
                }
                await _courseRecordRepository.UpdateAsync(record);
            }
        }

        /// <summary>
        /// 修改一个UserCourseRecordDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateUserCourseRecordDetailInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.UserId = input.UserId;
			   dbmodel.CourseId = input.CourseId;
			   dbmodel.LearningTime = input.LearningTime;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
		// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}