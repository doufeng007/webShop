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
    public class UserCourseCommentAppService : FRMSCoreAppServiceBase, IUserCourseCommentAppService
    { 
        private readonly IRepository<UserCourseComment, Guid> _repository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly ICourseSettingAppService _courseSettingAppService;
        private readonly IUserTrainScoreRecordAppService _trainScoreRecordAppService;
        private readonly IRepository<UserTrainScoreRecord,Guid> _trainScoreRecordRepository;
        public UserCourseCommentAppService(IRepository<UserCourseComment, Guid> repository
            , IRepository<Course, Guid> courseRepository, ICourseSettingAppService courseSettingAppService,
            IUserTrainScoreRecordAppService trainScoreRecordAppService, IRepository<UserTrainScoreRecord, Guid> trainScoreRecordRepository

        )
        {
            this._repository = repository;
            _courseRepository = courseRepository;
            _courseSettingAppService = courseSettingAppService;
            _trainScoreRecordAppService = trainScoreRecordAppService;
            _trainScoreRecordRepository = trainScoreRecordRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<UserCourseCommentListOutputDto>> GetList(GetUserCourseCommentListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.CourseId==input.CourseId)
			            join b in base.UserManager.Users on a.CreatorUserId equals b.Id
                        select new UserCourseCommentListOutputDto()
                        {
                            Id = a.Id,
                            UserName = b.Name,
                            CourseId = a.CourseId,
                            Comment = a.Comment,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<UserCourseCommentListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<UserCourseCommentOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<UserCourseCommentOutputDto>();
		}

        /// <summary>
        /// 添加一个UserCourseComment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateUserCourseCommentInput input)
        {
            var model = await _courseRepository.FirstOrDefaultAsync(x => x.Id == input.CourseId && x.Status == -1);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "课程不存在！");
            }
            var userId = (await base.GetCurrentUserAsync()).Id;
            var newmodel = new UserCourseComment()
            {
                UserId = userId,
                CourseId = input.CourseId,
                Comment = input.Comment
            };
            await _repository.InsertAsync(newmodel);
            //送积分
            var set = await _courseSettingAppService.GetSetVal(model.LearnType, model.IsSpecial);
            if (set.CommentScore > 0)
            {
                await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                {
                    FromId = input.CourseId,
                    FromType = TrainScoreFromType.CourseComment,
                    BusinessId = newmodel.Id,
                    Score = set.CommentScore,
                    UserId = userId
                });
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            var comment = await _repository.GetAsync(input.Id);
            var userId = (await base.GetCurrentUserAsync()).Id;
            var model = await _courseRepository.FirstOrDefaultAsync(x => x.Id == comment.CourseId);
            await _repository.DeleteAsync(x => x.Id == input.Id);
            //扣积分
            var set = await _trainScoreRecordRepository.FirstOrDefaultAsync(x => x.BusinessId == comment.Id);
            if (set != null)
            {
                await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                {
                    FromId = model.Id,
                    FromType = TrainScoreFromType.CourseComment,
                    Score = -set.Score,
                    UserId = userId
                });
            }
        }
    }
}