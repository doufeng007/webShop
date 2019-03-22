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
using Hangfire;
using Train.Enum;
using Train.Service.UserCourseRecord.Dto;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Train
{
    public class UserCourseRecordAppService : FRMSCoreAppServiceBase, IUserCourseRecordAppService
    { 
        private readonly IRepository<UserCourseRecord, Guid> _repository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<UserCourseRecordDetail, Guid> _courseRecordDetailRepository;
        private readonly IRepository<UserTrainScoreRecord, Guid> _trainScoreRecordRepository;
        private readonly ICourseSettingAppService _courseSettingAppService;
        private readonly ICacheManager _cacheManager;
        private readonly ICourseAppService _courseAppService;
        private readonly IUserCourseRecordNumberAppService _numberAppService;
        private readonly IUserTrainScoreRecordAppService _trainScoreRecordAppService;
        public UserCourseRecordAppService(IRepository<UserCourseRecord, Guid> repository
            , IRepository<Course, Guid> courseRepository,
            IAbpFileRelationAppService abpFileRelationAppService, ICacheManager cacheManager,
            IRepository<UserCourseRecordDetail, Guid> courseRecordDetailRepository, ICourseSettingAppService courseSettingAppService
            , IRepository<UserTrainScoreRecord, Guid> trainScoreRecordRepository, ICourseAppService courseAppService, IUserCourseRecordNumberAppService numberAppService
            , IUserTrainScoreRecordAppService trainScoreRecordAppService

        )
        {
            this._repository = repository;
            _courseRepository = courseRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _cacheManager = cacheManager;
            _courseRecordDetailRepository = courseRecordDetailRepository;
            _trainScoreRecordRepository = trainScoreRecordRepository;
            _courseSettingAppService = courseSettingAppService;
            _courseAppService = courseAppService;
            _numberAppService = numberAppService;
            _trainScoreRecordAppService = trainScoreRecordAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserCourseRecordListOutputDto>> GetList(GetUserCourseRecordListInput input)
        {
            var userId = (await base.GetCurrentUserAsync()).Id;
            var query = GetQueryFromList(userId, input);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var courseSet = await _courseSettingAppService.Get();
            ret.ForEach(x =>
            {
                x.FaceUrl = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
                {
                    BusinessId = x.CourseId.ToString(),
                    BusinessType = (int) AbpFileBusinessType.培训课程封面
                }).FirstOrDefault()?.Id.ToString();
                if (x.CourseFileType == CourseFileType.Doc || x.CourseFileType == CourseFileType.Pdf)
                {
                    x.CourseLink = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
                    {
                        BusinessId = x.CourseId.ToString(),
                        BusinessType = (int)AbpFileBusinessType.培训课程文件
                    }).FirstOrDefault()?.Id.ToString();
                }
                x.LearnScore = courseSet.GetSetVal(x.LearnType, x.IsSpecial).ClassHourScore;
                x.ViewingRatio = Math.Round(Convert.ToDecimal(x.MyLearnTime) / Convert.ToDecimal(x.LearnTime) * 100, 2);
            });
            return new PagedResultDto<UserCourseRecordListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 组装我的课程查询语句
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        IQueryable<UserCourseRecordListOutputDto> GetQueryFromList(long userId, GetUserCourseRecordListInput input)
        {
            var userIdStr = "u_" + userId.ToString();
            switch (input.type)
            {
                case MyCourseListType.RequiredCourse://我的必修课程
                    return
                    (from a in _courseRepository.GetAll().Where(x =>
                            !x.IsDeleted && (x.LearnType == CourseLearnType.MustAll ||
                                             (x.LearnType == CourseLearnType.Must &&
                                              x.LearnUser.GetStrContainsArray(userIdStr))) && x.Status == -1 && !x.IsDelCourse)
                        join b1 in _repository.GetAll()
                            .Where(x => !x.IsDeleted && x.UserId == userId)
                            .DefaultIfEmpty() on a.Id equals b1.CourseId into tmp1
                        from b in tmp1.DefaultIfEmpty()
                        let myLearnTime = _courseRecordDetailRepository.GetAll()
                            .Where(x => !x.IsDeleted && x.UserId == userId && x.CourseId == a.Id)
                            .OrderByDescending(x => (x.LastModificationTime ?? x.CreationTime))
                            .Select(x => x.LearningTime).FirstOrDefault()
                        select new UserCourseRecordListOutputDto()
                        {
                            CourseId = a.Id,
                            CourseName = a.CourseName,
                            CourseIntroduction =a.CourseIntroduction,
                            LearnTime = a.LearnTime,
                            MyLearnTime = myLearnTime,
                            IsComplate = b != null && (b.IsComplete != null && b.IsComplete != false),
                            LearnType = a.LearnType,
                            IsSpecial = a.IsSpecial,
                            CourseFileType = a.CourseFileType,
                            CourseLink = a.CourseLink,
                            CreationTime = a.CreationTime,
                            ComplateTime = a.ComplateTime
                        }).Where(x => x.IsComplate == false);
                case MyCourseListType.ElectiveCourse://我的选修课程
                    return (from a in _courseRepository.GetAll().Where(x =>
                            !x.IsDeleted && x.LearnType == CourseLearnType.Selected && x.Status == -1 && !x.IsDelCourse
                            && (input.CourseType == null || x.CourseType == input.CourseType)
                            && (input.CourseName == null || x.CourseName.Contains(input.CourseName))
                        )
                        join b1 in _repository.GetAll()
                            .Where(x => !x.IsDeleted && x.UserId == userId)
                            .DefaultIfEmpty() on a.Id equals b1.CourseId into tmp1
                        from b in tmp1.DefaultIfEmpty()
                            let myLearnTime = _courseRecordDetailRepository.GetAll()
                                .Where(x => !x.IsDeleted && x.UserId == userId && x.CourseId == a.Id)
                                .OrderByDescending(x => (x.LastModificationTime ?? x.CreationTime))
                                .Select(x => x.LearningTime).FirstOrDefault()
                            select new UserCourseRecordListOutputDto()
                            {
                                CourseId = a.Id,
                                CourseName = a.CourseName,
                                CourseIntroduction = a.CourseIntroduction,
                                LearnTime = a.LearnTime,
                                MyLearnTime = myLearnTime,
                                IsComplate = b != null && (b.IsComplete != null && b.IsComplete != false),
                                LearnType = a.LearnType,
                                IsSpecial = a.IsSpecial,
                                CourseFileType = a.CourseFileType,
                                CourseLink = a.CourseLink,
                                CreationTime = a.CreationTime
                            }).Where(x => x.IsComplate == false);
                case MyCourseListType.ComplateCourse://我的已完成课程
                    return (from a in _courseRepository.GetAll().Where(x =>
                            !x.IsDeleted && (x.LearnType != CourseLearnType.Must ||
                                             (x.LearnType == CourseLearnType.Must &&
                                              x.LearnUser.GetStrContainsArray(userIdStr))) && x.Status == -1 && !x.IsDelCourse)
                            join b in _repository.GetAll()
                                .Where(x => !x.IsDeleted && x.UserId == userId && x.IsComplete == true) on a.Id equals b
                                .CourseId
                            let myLearnTime = _courseRecordDetailRepository.GetAll()
                                .Where(x => !x.IsDeleted && x.UserId == userId && x.CourseId == a.Id)
                                .OrderByDescending(x => (x.LastModificationTime ?? x.CreationTime))
                                .Select(x => x.LearningTime).FirstOrDefault()
                            select new UserCourseRecordListOutputDto()
                            {
                                CourseId = a.Id,
                                CourseName = a.CourseName,
                                CourseIntroduction = a.CourseIntroduction,
                                LearnTime = a.LearnTime,
                                MyLearnTime = myLearnTime,
                                IsComplate = true,
                                LearnType = a.LearnType,
                                IsSpecial = a.IsSpecial,
                                CourseFileType = a.CourseFileType,
                                CourseLink = a.CourseLink,
                                CreationTime = b.CreationTime
                            });
            }
            return null;
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<UserCourseRecordOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = await _courseRepository.FirstOrDefaultAsync(x => x.Id == input.Id.Value && x.Status == -1 && !x.IsDelCourse);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "课程不存在！");
            }
            var userId = (await base.GetCurrentUserAsync()).Id;
            var record =
                await _repository.FirstOrDefaultAsync(x =>
                    !x.IsDeleted && x.CourseId == model.Id && x.UserId == userId);
            if (record == null)
            {
                record = new UserCourseRecord()
                {
                    CourseId = model.Id,
                    UserId = userId,
                    IsComplete = null,
                    Score = 0,
                    LearnTime = 0
                };
                await _repository.InsertAsync(record);
            }
            var result = model.MapTo<UserCourseRecordOutputDto>();
            result.FavorState = record.IsFavor == null
                ? CourseFavorState.None
                : record.IsFavor == false
                    ? CourseFavorState.Diss
                    : CourseFavorState.Favor;
            //获取总的赞踩情况
            var allFavorStates = await _repository.GetAll()
                .Where(x => !x.IsDeleted && x.CourseId == model.Id && x.IsFavor != null).Select(x => x.IsFavor)
                .ToListAsync();
            result.AllFavor = allFavorStates.Count(x => x == true);
            result.AllDiss = allFavorStates.Count(x => x == false);
            switch (result.CourseFileType)
            {
                case CourseFileType.Doc:
                case CourseFileType.Pdf:
                    result.CourseLink = (await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
                    {
                        BusinessId = model.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.培训课程文件
                    })).FirstOrDefault()?.Id.ToString();
                    break;
            }
            //获取我的学习时长
            var myRecord =await _courseRecordDetailRepository.GetAll()
                .Where(x => x.CourseId == model.Id && !x.IsDeleted && x.UserId == userId)
                .OrderByDescending(x => x.LastModificationTime ?? x.CreationTime).FirstOrDefaultAsync();
            if (myRecord != null)
            {
                result.MyLearnTime = myRecord.LearningTime;
            }
            //每次访问get必定上传一次观看次数
            await _numberAppService.Create(new CreateUserCourseRecordNumberInput() { CourseId = model.Id, UserId = userId });
            return result;
        }

        /// <summary>
        /// 添加一个UserCourseRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateUserCourseRecordInput input)
        {
                var newmodel = new UserCourseRecord()
                {
                    UserId = input.UserId,
                    CourseId = input.CourseId,
                    IsFavor = input.IsFavor,
                    IsComplete = input.IsComplete,
                    Score = 0,
                    LearnTime = 0
                };
				
                await _repository.InsertAsync(newmodel);
				
        }

        /// <summary>
        /// 修改一个UserCourseRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateUserCourseRecordInput input)
        {
            if (input.CourseId != Guid.Empty)
            {
                var userId = (await base.GetCurrentUserAsync()).Id;
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.CourseId == input.CourseId && x.UserId== userId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                switch (input.FavorState)
                {
                    case CourseFavorState.None:
                        dbmodel.IsFavor = null;
                        break;
                    case CourseFavorState.Diss:
                        dbmodel.IsFavor = false;
                        break;
                    case CourseFavorState.Favor:
                        dbmodel.IsFavor = true;
                        break;
                }
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        ///<summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }

        /// <summary>
        /// 我的统计积分
        /// </summary>
        /// <returns></returns>
        public async Task<UserCourseStatisticsOutputDto> GetMyStatisticsScore(bool refresh = false, long? userId = null)
        {
            var myUserId = userId ?? (await base.GetCurrentUserAsync()).Id;
            var cache = _cacheManager.GetCache("UserCourseStatistics");
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(1);
            if (refresh)
            {
                await cache.RemoveAsync($"{myUserId}");
            }
            return await cache.GetAsync(myUserId, async () => await GetMyStatisticsScoreFromDb(myUserId));
        }

        /// <summary>
        /// 我的统计积分-查询db
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<UserCourseStatisticsOutputDto> GetMyStatisticsScoreFromDb(long userId)
        {
            var result = new UserCourseStatisticsOutputDto();
            //我的统计积分来自于必修课程与选修课程，必修课程的积分来自于全员必修和部分必修
            //先查询所有已修完毕的记录
            var complateSourceList = await (from b in _repository.GetAll().Where(x => !x.IsDeleted && x.UserId == userId && x.IsComplete == true)
                                            join c in _courseRepository.GetAll().Where(x => !x.IsDeleted && x.Status == -1 && !x.IsDelCourse) on b.CourseId equals c.Id
                select new
                {
                    c.LearnType
                }).ToListAsync();
            //我的已修必修课数
            result.RequiredComplateSourceScore = complateSourceList
                .Count(x => x.LearnType == CourseLearnType.Must || x.LearnType == CourseLearnType.MustAll);
            //所有必修数
            var userIdStr = "u_" + userId;
            result.RequiredAllSourceScore = await (from a in _courseRepository.GetAll().Where(x =>
                    !x.IsDeleted && !x.IsDelCourse &&
                    ((x.LearnType == CourseLearnType.Must && x.LearnUser.GetStrContainsArray(userIdStr)) ||
                     x.LearnType == CourseLearnType.MustAll) && x.Status == -1)
                select a
            ).CountAsync();
            //我的已修选修课数
            result.ElectiveComplateSourceScore = complateSourceList
                .Count(x => x.LearnType == CourseLearnType.Selected);
            //所有我观看过的选修数
            var setElectiveIsSpecial = (await _courseSettingAppService.GetSetVal(CourseLearnType.Selected, true))
                .ClassHourScore;
            var setElectiveNotSpecial = (await _courseSettingAppService.GetSetVal(CourseLearnType.Selected, false))
                .ClassHourScore;
            result.ElectiveAllSourceScore =
                await (from a in _repository.GetAll().Where(x => !x.IsDeleted && x.UserId == userId)
                    join c in _courseRepository.GetAll()
                        .Where(x => !x.IsDeleted && !x.IsDelCourse && x.LearnType == CourseLearnType.Selected && x.Status == -1) on a.CourseId equals c.Id
                    select c.IsSpecial ? setElectiveIsSpecial : setElectiveNotSpecial
                ).CountAsync();
            //我的课数
            result.MyAllSourceScore = result.RequiredAllSourceScore + result.ElectiveAllSourceScore;
            result.MyComplateSourceScore = result.RequiredComplateSourceScore + result.ElectiveComplateSourceScore;
            //学习时长
            result.LearnTime = await _courseRecordDetailRepository.GetAll()
                .Where(x => !x.IsDeleted && x.UserId == userId)
                .Select(x => new
                {
                    x.LearningTime,
                    x.CourseId,
                }).GroupBy(x => x.CourseId)
                .Select(x => x.OrderByDescending(y => y.LearningTime).FirstOrDefault().LearningTime).SumAsync();
            //本周排名
            var myRank = await _courseAppService.GetCourseWeekRankById(userId);
            result.ThisWeekRank = myRank?.Rank ?? -1;
            return result;
        }

        /// <summary>
        /// 执行减扣积分的后台任务
        /// </summary>
        /// <param name="courseId"></param>
        public async Task DeductionScore(Guid courseId)
        {
            //当用户必修课程在规定时间内没有修习完成，则需要扣除积分
            var course = await _courseRepository.GetAsync(courseId);
            if (course != null && course.LearnType != CourseLearnType.Selected && course.ComplateTime < DateTime.Now)
            {
                var chooseUserId = new List<long>();
                var failRecord = new List<UserCourseRecord>();
                switch (course.LearnType)
                {
                    case CourseLearnType.Must:
                        chooseUserId = course.LearnUser.Replace("u_", "").Split(",").Select(long.Parse).ToList();
                        break;
                    case CourseLearnType.MustAll:
                        chooseUserId = UserManager.Users.Select(x => x.Id).ToList();
                        break;
                }
                //查询有课程记录的人
                var learnRecord = await _repository.GetAll()
                    .Where(x => !x.IsDeleted && chooseUserId.Contains(x.UserId) && x.CourseId == courseId)
                    .ToListAsync();
                //将所有人分为两组，有记录但未完成课程的人，无记录的人
                //未完成记录
                failRecord.AddRange(learnRecord.Where(x => x.IsComplete == null));
                //无记录的人
                var noRecordUserId = chooseUserId.Except(learnRecord.Select(x => x.UserId)).ToList();
                //将有课程记录但未完成的人标记为失败
                failRecord.ToList().ForEach(async x =>
                {
                    x.IsComplete = false;
                    await _repository.UpdateAsync(x);
                });
                //将没有课程记录的人增加课程记录并标记为失败
                noRecordUserId.ForEach(async x =>
                {
                    var norecord = new UserCourseRecord()
                    {
                        CourseId = course.Id,
                        UserId = x,
                        IsComplete = false,
                        Score = 0,
                        LearnTime = 0
                    };
                    await _repository.InsertAsync(norecord);
                    //加入失败记录
                    failRecord.Add(norecord);
                });
                //扣除积分
                var setScore = await _courseSettingAppService.GetSetVal(course.LearnType, course.IsSpecial);
                failRecord.ForEach(async x =>
                {
                    if (setScore.ClassHourScore > 0)
                    {
                        await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                        {
                            FromType = TrainScoreFromType.CourseLearn,
                            FromId = course.Id,
                            Score = -setScore.ClassHourScore,
                            UserId = x.UserId,
                            BusinessId = x.Id
                        });
                    }
                });
            }
            else
            {
                //关闭当前任务
                RecurringJob.RemoveIfExists($"usercoursefail-{courseId}");
            }
        }
    }
}