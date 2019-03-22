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
using Abp.Dependency;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using Train.Enum;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Train
{
    public class UserTrainScoreRecordAppService : FRMSCoreAppServiceBase, IUserTrainScoreRecordAppService
    { 
        private readonly IRepository<UserTrainScoreRecord, Guid> _repository;
        private readonly IRepository<UserCourseRecord, Guid> _userCourseRecordRepository;
        public UserTrainScoreRecordAppService(IRepository<UserTrainScoreRecord, Guid> repository
            , IRepository<UserCourseRecord, Guid> userCourseRecordRepository

        )
        {
            this._repository = repository;
            _userCourseRecordRepository = userCourseRecordRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<UserTrainScoreRecordListOutputDto>> GetList(GetUserTrainScoreRecordListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new UserTrainScoreRecordListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            FromType = a.FromType,
                            FromId = a.FromId,
                            Score = a.Score,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<UserTrainScoreRecordListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<UserTrainScoreRecordOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<UserTrainScoreRecordOutputDto>();
		}
		/// <summary>
        /// 添加一个UserTrainScoreRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateUserTrainScoreRecordInput input)
        {
                var newmodel = new UserTrainScoreRecord()
                {
                    UserId = input.UserId,
                    FromType = input.FromType,
                    FromId = input.FromId,
                    Score = input.Score
		        };
            //将课程获取的积分增加到用户记录中
            if (input.FromType == TrainScoreFromType.CourseLearn ||
                input.FromType == TrainScoreFromType.CourseExperience ||
                input.FromType == TrainScoreFromType.CourseComment)
            {
                var userRecord = await _userCourseRecordRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.CourseId == input.FromId && x.UserId == input.UserId)
                    .FirstOrDefaultAsync();
                if (userRecord != null)
                {
                    userRecord.Score = userRecord.Score + input.Score;
                    await _userCourseRecordRepository.UpdateAsync(userRecord);
                }
                //更新用户缓存
                await Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IUserCourseRecordAppService>().GetMyStatisticsScore(true, input.UserId);
            }
            await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个UserTrainScoreRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateUserTrainScoreRecordInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.UserId = input.UserId;
			   dbmodel.FromType = input.FromType;
			   dbmodel.FromId = input.FromId;
			   dbmodel.Score = input.Score;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
		/// <summary>
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