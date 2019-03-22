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
using Abp.Runtime.Caching;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using Microsoft.AspNetCore.Mvc;
using Train.Enum;
using Train.Jobs;
using Train.Service.Course.Dto;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Train
{
    public class CourseAppService : FRMSCoreAppServiceBase, ICourseAppService
    { 
        private readonly IRepository<Course, Guid> _repository;
		private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectNoticeManager _noticeManager;
        private readonly IRepository<AbpDictionary, Guid> _dictionRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<UserTrainScoreRecord, Guid> _userTrainScoreRecordRepository;
        private readonly ICourseFailHangFire _courseFailHangFire;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly IRepository<UserCourseRecord, Guid> _userCourseRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;

        public CourseAppService(IRepository<Course, Guid> repository
		,WorkFlowBusinessTaskManager workFlowBusinessTaskManager,IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager
            , IRepository<AbpDictionary, Guid> dictionRepository, ProjectNoticeManager noticeManager, ICacheManager cacheManager
            , IRepository<UserTrainScoreRecord, Guid> userTrainScoreRecordRepository, ICourseFailHangFire courseFailHangFire, IRepository<WorkFlowTask, Guid> workFlowTaskRepository
            , IRepository<UserCourseRecord, Guid> userCourseRepository, IWorkFlowWorkTaskAppService workFlowWorkTaskAppService
        )
        {
            this._repository = repository;
			_workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
             _projectAuditManager = projectAuditManager;
             _workFlowCacheManager = workFlowCacheManager;
            _noticeManager = noticeManager;
            _dictionRepository = dictionRepository;
            _cacheManager = cacheManager;
            _userTrainScoreRecordRepository = userTrainScoreRecordRepository;
            _courseFailHangFire = courseFailHangFire;
            _workFlowTaskRepository = workFlowTaskRepository;
            _userCourseRepository = userCourseRepository;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CourseListOutputDto>> GetList(GetCourseListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted
			                                                    && (string.IsNullOrEmpty(input.CourseName) || x.CourseName.Contains(input.CourseName))
			                                                    && (input.IsComplate ? x.Status == -1 : x.Status >= 0)
                                                                && (input.CourseType == null || input.CourseType == x.CourseType)
                                                                && !x.IsDelCourse
                        )
			    join b1 in _dictionRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on a.CourseType equals b1.Id into tmp1
                from b in tmp1.DefaultIfEmpty()
			    let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
			            x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
			            x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new CourseListOutputDto()
                        {
                            Id = a.Id,
                            CourseName = a.CourseName,
                            CourseType = a.CourseType,
                            CourseTypeName = b == null ? "" : b.Title,
                            CourseLink = a.CourseLink,
                            CourseFileType = a.CourseFileType,
                            LearnTime = a.LearnTime,
                            Recommend = a.Recommend,
                            RecommendWords = a.RecommendWords,
                            CourseIntroduction = a.CourseIntroduction,
                            IsExperience = a.IsExperience,
                            LearnUser = a.LearnUser,
                            LearnType = a.LearnType,
                            ComplateTime = a.ComplateTime,
                            IsSpecial = a.IsSpecial,
                            FilePage = a.FilePage,
                            CreationTime = a.CreationTime,
                            DealWithUsers = a.DealWithUsers,
                            Status = a.Status,
                            CopyForUsers = a.CopyForUsers,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                item.CourseFile = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
                {
                    BusinessId = item.InstanceId,
                    BusinessType = (int) AbpFileBusinessType.培训课程文件
                });
            }
            return new PagedResultDto<CourseListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
		public async Task<CourseOutputDto> Get(GetWorkFlowTaskCommentInput input)
		{
			var id = Guid.Parse(input.InstanceId);
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
		    var result = model.MapTo<CourseOutputDto>();
		    result.CourseFile = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.培训课程文件 });
		    result.CourseCoverFile = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.培训课程封面 });
		    result.CourseTypeName = (await _dictionRepository.FirstOrDefaultAsync(x => x.Id == result.CourseType))?.Title;
            return result;
        }
        /// <summary>
        /// 添加一个Course
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCourseInput input)
        {
            var user = await base.GetCurrentUserAsync();
            if (input.LearnType == CourseLearnType.Must && string.IsNullOrEmpty(input.LearnUser))
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "必须填写指派人员");
            }
            var newmodel = new Course()
            {
                CourseName = input.CourseName,
                CourseType = input.CourseType,
                CourseLink = input.CourseLink,
                CourseFileType = input.CourseFileType,
                LearnTime = input.LearnTime,
                Recommend = user.Name,
                RecommendWords = input.RecommendWords,
                CourseIntroduction = input.CourseIntroduction,
                IsExperience = input.IsExperience,
                LearnUser = input.LearnUser ?? "",
                LearnType = input.LearnType,
                ComplateTime = input.ComplateTime?.Date.AddDays(1),
                IsSpecial = input.IsSpecial,
                FilePage = input.FilePage,
                DealWithUsers = input.DealWithUsers,
                Status = input.Status,
                CopyForUsers = input.CopyForUsers
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            await _abpFileRelationAppService.CreateListAsync(newmodel.Id.ToString(), AbpFileBusinessType.培训课程文件,
                input.CourseFile);
            await _abpFileRelationAppService.CreateListAsync(newmodel.Id.ToString(), AbpFileBusinessType.培训课程封面,
                input.CourseCoverFile);
            //增加后台扣积分的任务
            if (newmodel.ComplateTime != null && (newmodel.LearnType == CourseLearnType.Must || newmodel.LearnType == CourseLearnType.MustAll))
            {
                _courseFailHangFire.CreateJob(newmodel.Id);
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个Course
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCourseInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new Course();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<Course>();
                }
                dbmodel.CourseName = input.CourseName;
                dbmodel.CourseType = input.CourseType;
                dbmodel.CourseLink = input.CourseLink;
                dbmodel.CourseFileType = input.CourseFileType;
                dbmodel.LearnTime = input.LearnTime;
                dbmodel.Recommend = input.Recommend;
                dbmodel.RecommendWords = input.RecommendWords;
                dbmodel.CourseIntroduction = input.CourseIntroduction;
                dbmodel.IsExperience = input.IsExperience;
                dbmodel.LearnUser = input.LearnUser;
                dbmodel.LearnType = input.LearnType;
                dbmodel.ComplateTime = input.ComplateTime?.Date.AddDays(1);
                dbmodel.IsSpecial = input.IsSpecial;
                dbmodel.FilePage = input.FilePage;
                dbmodel.DealWithUsers = input.DealWithUsers;
                dbmodel.Status = input.Status;
                dbmodel.CopyForUsers = input.CopyForUsers;

                await _repository.UpdateAsync(dbmodel);
                await _abpFileRelationAppService.UpdateListAsync(dbmodel.Id.ToString(), AbpFileBusinessType.培训课程文件,
                    input.CourseFile);
                await _abpFileRelationAppService.UpdateListAsync(dbmodel.Id.ToString(), AbpFileBusinessType.培训课程封面,
                    input.CourseCoverFile);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.InStanceId.ToString(),
                        flowModel.TitleField.Table);
                }
                //增加后台扣积分的任务
                if (dbmodel.ComplateTime != null && (dbmodel.LearnType == CourseLearnType.Must || dbmodel.LearnType == CourseLearnType.MustAll))
                {
                    _courseFailHangFire.CreateJob(dbmodel.Id);
                }
                //若课程不为强制必读了，则删除后台扣积分的任务
                else if (dbmodel.ComplateTime == null || dbmodel.LearnType == CourseLearnType.Selected)
                {
                    _courseFailHangFire.RemoveJob(dbmodel.Id);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        private CourseLogDto GetChangeModel(Course model)
        {
            var ret = model.MapTo<CourseLogDto>();
            return ret;
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [HttpPost]
        public async Task Delete(CourseDelInput input)
        {
            //查询录入流程已经走完并且没有人学习中的课程
            var dbmodels = await (from a in _repository.GetAll().Where(x => input.Id.Contains(x.Id))
                let b = (from c in _userCourseRepository.GetAll().Where(x => x.CourseId == a.Id) select c.Id).Any()
                select new
                {
                    course = a,
                    userlearn = b
                }
            ).ToListAsync();
            if (dbmodels.Any())
            {
                var delmodel = new List<Course>();
                var nodelCourse = new Course();
                dbmodels.ForEach(x =>
                {
                    if (x.course.Status == -1 && !x.userlearn)
                    {
                        x.course.IsDelCourse = true;
                        delmodel.Add(x.course);
                    }
                    else
                    {
                        nodelCourse = nodelCourse.Id == Guid.Empty ? x.course : nodelCourse;
                    }
                });
                if (nodelCourse.Id != Guid.Empty)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"课程\"{nodelCourse.CourseName}\"已有用户在学习，无法删除!");
                }
                if (delmodel.Any())
                {
                    delmodel.ForEach(async x =>
                    {
                        await _repository.UpdateAsync(x);
                    });
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"请至少选择一门要删除的课程!");
            }
        }


        /// <summary>
        /// 课程推荐成功后给推荐人发消息
        /// </summary>
        /// <param name="eventParams"></param>
        public  void SendMessageForRecommendUser(WorkFlowCustomEventParams eventParams)
        {
            var id = Guid.Parse(eventParams.InstanceID);
            var recommend = _repository.Get(id);
            if (recommend != null && (recommend.CreatorUserId ?? 0) != 0)
            {
                 _noticeManager.CreateOrUpdateNotice(new NoticePublishInput()
                {
                    Title = "课程推荐回复通知",
                    Content = "感谢您提供的宝贵资源，我们已经收到，期待您更多的分享",
                    NoticeUserIds = recommend.CreatorUserId.ToString(),
                    NoticeType = 1
                });
            }
        }

        /// <summary>
        /// 获取周排行榜
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CourseWeekRankOutputDto>> GetCourseWeekRank()
        {
            var cache = _cacheManager.GetCache("CourseWeekRank");
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(1);
            return await cache.GetAsync("CourseWeekRankKey", async () => await GetCourseWeekRankFromDb());
        }

        /// <summary>
        /// 获取周排行榜数据库
        /// </summary>
        /// <returns></returns>
        private async Task<List<CourseWeekRankOutputDto>> GetCourseWeekRankFromDb()
        {
            var startTime = DateTime.Now.GetMondayDate();
            var endTime = DateTime.Now.GetSundayDate().AddDays(1);
            var rankList = await (from a in _userTrainScoreRecordRepository.GetAll().Where(
                    x => !x.IsDeleted &&
                         (x.FromType == TrainScoreFromType.CourseLearn ||
                          x.FromType == TrainScoreFromType.CourseComment ||
                          x.FromType == TrainScoreFromType.CourseExperience) &&
                         (x.CreationTime >= startTime && x.CreationTime < endTime)
                )
                join b in UserManager.Users on a.UserId equals b.Id
                select new
                {
                    a.Score,
                    a.UserId,
                    b.Name
                }).GroupBy(x => x.UserId).Select(x => new CourseWeekRankOutputDto
            {
                Score = x.Select(y => y.Score).Sum(),
                UserId = x.Key,
                Name = x.Select(y => y.Name).FirstOrDefault()
            }).OrderByDescending(x => x.Score).Take(10).ToListAsync();
            var rank = 1;
            rankList.ForEach(x =>
            {
                x.Rank = rank;
                rank++;
            });
            return rankList;
        }

        /// <summary>
        /// 获取当前用户的周排行
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CourseWeekRankOutputDto> GetCourseWeekRankById(long userId)
        {
            var rankList = await GetCourseWeekRank();
            return rankList.FirstOrDefault(x => x.UserId == userId);
        }

        /// <summary>
        /// 获取我的推荐课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MyCourseRecommandOutput>> GetMyCourseRecommand(MyCourseRecommandInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var query = (from a in _repository.GetAll().Where(x => !x.IsDeleted && x.CreatorUserId == user.Id)
                let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                        x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                        x.ReceiveID == AbpSession.UserId.Value)
                    select c)
                select new MyCourseRecommandOutput
                {
                    Id = a.Id,
                    CourseName = a.CourseName,
                    CourseFileType = a.CourseFileType,
                    RecommendWords = a.RecommendWords,
                    CourseLink = a.CourseLink,
                    CreationTime = a.CreationTime,
                    Status = a.Status ?? 0,
                    OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                        ? 1
                        : 2
                });

            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                item.CourseFile = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
                {
                    BusinessId = item.InstanceId,
                    BusinessType = (int)AbpFileBusinessType.培训课程文件
                });
                item.CommentList= _workFlowWorkTaskAppService.GetInstanceComment(new GetWorkFlowTaskCommentInput() { FlowId = input.FlowId, InstanceId = item.InstanceId });
            }
            return new PagedResultDto<MyCourseRecommandOutput>(toalCount, ret);
        }

        public async Task DelCourseType(Guid id)
        {
            var dict = await _dictionRepository.GetAsync(id);
            if (dict == null)
            {
                throw new UserFriendlyException((int) ErrorCode.CodeValErr, $"课程分类不存在，无法删除!");
            }
            if (await _repository.GetAll().AnyAsync(x => !x.IsDelCourse && !x.IsDeleted && x.CourseType == id))
            {
                throw new UserFriendlyException((int) ErrorCode.CodeValErr, $"课程分类\"{dict.Title}\"已有课程在使用，无法删除!");
            }
            dict.IsDeleted = true;
            await _dictionRepository.UpdateAsync(dict);
        }
    }
}