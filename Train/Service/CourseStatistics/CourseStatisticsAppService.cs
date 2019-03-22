using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.File;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.WorkFlowDictionary;
using Abp.Zero.Configuration;
using HR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using Train.Enum;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Common;
using ZCYX.FRMSCore.Extensions;

namespace Train
{
    public class CourseStatisticsAppService : FRMSCoreAppServiceBase, ICourseStatisticsAppService
    {
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<UserCourseRecord, Guid> _userCourseRecordrepository;
        private readonly IRepository<UserCourseRecordDetail, Guid> _courseRecordDetailRepository;
        private readonly IRepository<UserTrainScoreRecord, Guid> _trainScoreRecordRepository;
        private readonly IRepository<UserCourseRecordNumber, Guid> _courseRecordNumberRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<Employee, Guid> _emplpyeeRepository;
        private readonly IRepository<UserCourseComment, Guid> _userCourseCommentRepository;
        private readonly IRepository<AbpDictionary, Guid> _dictionRepository;
        private readonly IRepository<TrainUserExperience, Guid> _experienceRepository;
        private readonly IRepository<UserPosts, Guid> _userPostsrepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        public static int StatisiticCacheExpiryTime = 5;
        private static string CourseDictionCode = "kcfl";
        public static string StatisiticCacheName = "CourseStatistics";
        public CourseStatisticsAppService(IRepository<UserCourseRecord, Guid> userCourseRecordrepository
            , IRepository<Course, Guid> courseRepository,ICacheManager cacheManager,
            IRepository<UserCourseRecordDetail, Guid> courseRecordDetailRepository
            , IRepository<UserTrainScoreRecord, Guid> trainScoreRecordRepository
            , IRepository<UserCourseRecordNumber, Guid> courseRecordNumberRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<Employee, Guid> emplpyeeRepository, IRepository<UserCourseComment, Guid> userCourseCommentRepository
            , IRepository<AbpDictionary, Guid> dictionRepository, IRepository<TrainUserExperience, Guid> experienceRepository
            , IRepository<UserPosts, Guid> userPostsrepository, IRepository<PostInfo, Guid> postsRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository

        )
        {
            _userCourseRecordrepository = userCourseRecordrepository;
            _courseRepository = courseRepository;
            _cacheManager = cacheManager;
            _courseRecordDetailRepository = courseRecordDetailRepository;
            _trainScoreRecordRepository = trainScoreRecordRepository;
            _courseRecordNumberRepository = courseRecordNumberRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _emplpyeeRepository = emplpyeeRepository;
            _userCourseCommentRepository = userCourseCommentRepository;
            _dictionRepository = dictionRepository;
            _experienceRepository = experienceRepository;
            _userPostsrepository = userPostsrepository;
            _postsRepository = postsRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;

        }
        #region 学员统计

        /// <summary>
        /// 获取统计指标-学员统计
        /// </summary>
        /// <returns></returns>
        public async Task<RecommendedIndexOutputDto> GetRecommendedIndexByUser()
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync("GetRecommendedIndexByUser",
                async () =>
                {
                    var result = new RecommendedIndexOutputDto();

                    result.WatchPerson = await _userCourseRecordrepository.GetAll()
                        .Where(x => !x.IsDeleted && x.LearnTime > 0).GroupBy(x => x.UserId).CountAsync();

                    var allUser = Convert.ToDecimal(result.WatchPerson);
                    result.WatchNumber = await _courseRecordNumberRepository.CountAsync(x => !x.IsDeleted);

                    result.CourseCount =
                        await _userCourseRecordrepository.CountAsync(x => !x.IsDeleted && x.IsComplete == true);

                    result.AvgPersonCourseCount = allUser==0?0:
                        Math.Round(
                            Convert.ToDecimal(result.CourseCount) / allUser,
                            2);

                    result.CourseTimeCount = await _userCourseRecordrepository.GetAll().Where(x => !x.IsDeleted)
                        .SumAsync(x => (x.LearnTime ?? 0));

                    result.AvgPersonCourseTimeCount = allUser == 0 ? 0 :
                        Math.Round(
                            Convert.ToDecimal(result.CourseTimeCount) / allUser,
                            2);

                    result.CourseScoreCount = await _userCourseRecordrepository.GetAll().Where(x => !x.IsDeleted)
                        .SumAsync(x => (x.Score ?? 0));

                    result.AvgCourseScoreCount = allUser == 0 ? 0 :
                        Math.Round(
                            Convert.ToDecimal(result.CourseScoreCount) / allUser,
                            2);
                    return result;
                });
        }

        /// <summary>
        /// 根据部门统计学员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DepartmentOutputDto>> GetDepartmentCourseStatistics(DepartmentInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync($"GetDepartmentCourseStatistics{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    var minTime = DateTime.Parse($"{input.StatisticYear}-{input.StatisticMonth}-01");
                    var maxTime = minTime.AddMonths(1);
                    var query = (from a in UserManager.Users.Where(x =>
                            string.IsNullOrEmpty(input.UserName) || x.Name.Contains(input.UserName))
                                 join b in _userOrganizationUnitRepository.GetAll().Where(x => !x.IsDeleted && x.IsMain) on a.Id
                                     equals
                                     b.UserId
                                 join c in _organizationUnitRepository.GetAll().Where(
                                         x => !x.IsDeleted && (input.DepartmentId == null || x.Id == input.DepartmentId)
                                     ) on
                                     b.OrganizationUnitId equals c.Id
                                 join f in _emplpyeeRepository.GetAll().Where(x =>
                                     !x.IsDeleted && (input.StartTime == null || x.EnterTime >= input.StartTime)
                                     && (input.EndTime == null || x.EnterTime <= input.EndTime)) on a.Id equals f.UserId
                                 let courseNumber = (
                                     from d in _trainScoreRecordRepository.GetAll().Where(x =>
                                         !x.IsDeleted && x.UserId == a.Id &&
                                         x.FromType == TrainScoreFromType
                                             .CourseLearn
                                         && x.CreationTime >= minTime &&
                                         x.CreationTime < maxTime
                                     )
                                     select d.Id).Count()
                                 let courseTimeNumber = (from g in _userCourseRecordrepository.GetAll().Where(x =>
                                         !x.IsDeleted && x.UserId == a.Id 
                                         && (x.LastModificationTime ?? x.CreationTime) >= minTime 
                                         && (x.LastModificationTime ?? x.CreationTime) < maxTime)
                                                         select g.LearnTime ?? 0
                                 ).Sum()
                                 let scoreRecord = (
                                     from d in _trainScoreRecordRepository.GetAll().Where(x =>
                                         !x.IsDeleted && x.UserId == a.Id &&
                                         (x.FromType == TrainScoreFromType
                                              .CourseLearn ||
                                          x.FromType == TrainScoreFromType
                                              .CourseExperience ||
                                          x.FromType == TrainScoreFromType
                                              .CourseComment)
                                         && x.CreationTime >= minTime &&
                                         x.CreationTime < maxTime)
                                     select d.Score).Sum()
                                 let watchNumber = (from e in _courseRecordNumberRepository.GetAll()
                                         .Where(x => !x.IsDeleted && x.UserId == a.Id && x.CreationTime >= minTime &&
                                                     x.CreationTime < maxTime)
                                                    select e.Id).Count()
                                 select new DepartmentOutputDto
                                 {
                                     UserId = a.Id,
                                     Name = a.Name,
                                     CourseNumber = courseNumber,
                                     CourseTimeNumber = courseTimeNumber,
                                     Score = scoreRecord,
                                     WatchNumber = watchNumber,
                                     ChargeLeader = c.ChargeLeader
                                 }).Where(x => input.IsLeader == null || (input.IsLeader == false? !x.ChargeLeader.GetStrContainsArray("u_" + x.UserId): x.ChargeLeader.GetStrContainsArray("u_" + x.UserId)));
                    var toalCount = await query.CountAsync();
                    var ret = await query.OrderByDescending(r => r.UserId).PageBy(input).ToListAsync();
                    var index = input.SkipCount + 1;
                    ret.ForEach(x =>
                    {
                        x.Index = index;
                        index++;
                    });
                    return new PagedResultDto<DepartmentOutputDto>(toalCount, ret);
                });
        }

        /// <summary>
        /// 根据学员id获取学员课程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DepartmentUserOutputDto>> GetDepartmentUserCourseStatistics(
            DepartmentUserInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync($"GetDepartmentUserCourseStatistics{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    var minTime = DateTime.Parse($"{input.StatisticYear}-{input.StatisticMonth}-01");
                    var maxTime = minTime.AddMonths(1);
                    var query = (from a in UserManager.Users.Where(x => x.Id == input.UserId)
                        join b in _userCourseRecordrepository.GetAll().Where(x =>
                            !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime) on a.Id equals b
                            .UserId
                        join c in _courseRepository.GetAll().Where(x => !x.IsDeleted) on b.CourseId equals c.Id
                        let watchTime = (from d in _courseRecordNumberRepository.GetAll()
                                .Where(x => !x.IsDeleted && x.CourseId == c.Id && x.UserId == a.Id &&
                                            x.CreationTime >= minTime && x.CreationTime < maxTime)
                                .OrderByDescending(x => x.CreationTime)
                            select d.CreationTime)
                        select new DepartmentUserOutputDto
                        {
                            CourseName = c.CourseName,
                            LearnState = b.IsComplete == null ? 0 : b.IsComplete == false ? 1 : 2,
                            StartTime = b.CreationTime,
                            LastWatchTime = watchTime.FirstOrDefault(),
                            WatchNumber = watchTime.Count()
                        });
                    var toalCount = await query.CountAsync();
                    var ret = await query.OrderByDescending(r => r.LearnState).PageBy(input).ToListAsync();
                    return new PagedResultDto<DepartmentUserOutputDto>(toalCount, ret);
                });
        }
        /// <summary>
        /// 获取部门统计图表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<DepartmentImageOutputDto>> GetRecommendedIndexImageByUser(DepartmentImageInput input)
        {

            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync($"GetRecommendedIndexImageByUser{JsonConvert.SerializeObject(input).GetMd5()}",
                   async () =>
                   {
                       IQueryable<DepartmentImageUserOutputDto> GetQueryByUserIndexImage()
                       {
                           var minTime = DateTime.Parse($"{input.StatisticYear}-{input.StatisticMonth}-01");
                           var maxTime = minTime.AddMonths(1);
                           switch (input.Type)
                           {
                               case DepartmentStatisticType.WatchPerson: //观看人数
                                    return (from a in _courseRecordNumberRepository.GetAll().Where(x =>
                                                !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                           join b in _courseRepository.GetAll().Where(x =>
                                                   !x.IsDeleted && (input.IsMustLearn == null || (input.IsMustLearn == false
                                                                        ? x.LearnType == CourseLearnType.Selected
                                                                        : (x.LearnType == CourseLearnType.Must ||
                                                                           x.LearnType == CourseLearnType.MustAll))))
                                               on a.CourseId equals b.Id
                                           group a by a.UserId
                                            into g
                                           select new DepartmentImageUserOutputDto
                                           {
                                               UserId = g.Key,
                                               Number = 1
                                           });
                               case DepartmentStatisticType.WatchNumber: //观看次数
                                    return (from a in _courseRecordNumberRepository.GetAll().Where(x =>
                                                !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                           join b in _courseRepository.GetAll().Where(x =>
                                                   !x.IsDeleted && (input.IsMustLearn == null || (input.IsMustLearn == false
                                                                        ? x.LearnType == CourseLearnType.Selected
                                                                        : (x.LearnType == CourseLearnType.Must ||
                                                                           x.LearnType == CourseLearnType.MustAll))))
                                               on a.CourseId equals b.Id
                                           group a by a.UserId
                                            into g
                                           select new DepartmentImageUserOutputDto
                                           {
                                               UserId = g.Key,
                                               Number = g.Count()
                                           });
                               case DepartmentStatisticType.CourseCount: //课程数
                                case DepartmentStatisticType.AvgPersonCourseCount:
                                   return (from a in _trainScoreRecordRepository.GetAll().Where(x =>
                                           !x.IsDeleted && x.FromType == TrainScoreFromType.CourseLearn &&
                                           x.CreationTime >= minTime && x.CreationTime < maxTime)
                                           join b in _courseRepository.GetAll().Where(x =>
                                                   !x.IsDeleted && (input.IsMustLearn == null || (input.IsMustLearn == false
                                                                        ? x.LearnType == CourseLearnType.Selected
                                                                        : (x.LearnType == CourseLearnType.Must ||
                                                                           x.LearnType == CourseLearnType.MustAll))))
                                               on a.FromId equals b.Id
                                           group a by a.UserId
                                       into g
                                           select new DepartmentImageUserOutputDto
                                           {
                                               UserId = g.Key,
                                               Number = g.Count()
                                           });
                               case DepartmentStatisticType.CourseTimeCount: //课时数
                                case DepartmentStatisticType.AvgPersonCourseTimeCount:
                                   return (from a in _courseRecordDetailRepository.GetAll().Where(x =>
                                           !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                           join b in _courseRepository.GetAll().Where(x =>
                                                   !x.IsDeleted && (input.IsMustLearn == null || (input.IsMustLearn == false
                                                                        ? x.LearnType == CourseLearnType.Selected
                                                                        : (x.LearnType == CourseLearnType.Must ||
                                                                           x.LearnType == CourseLearnType.MustAll))))
                                               on a.CourseId equals b.Id
                                           group a by new { a.UserId, a.CourseId }
                                       into g
                                           select new DepartmentImageUserOutputDto
                                           {
                                               UserId = g.Key.UserId,
                                               Number = g.Max(x => x.LearningTime)
                                           });

                               case DepartmentStatisticType.CourseScoreCount: //获得积分
                                case DepartmentStatisticType.AvgCourseScoreCount:
                                   return (from a in _trainScoreRecordRepository.GetAll().Where(x =>
                                           !x.IsDeleted &&
                                           x.CreationTime >= minTime && x.CreationTime < maxTime)
                                           join b in _courseRepository.GetAll().Where(x =>
                                                   !x.IsDeleted && (input.IsMustLearn == null || (input.IsMustLearn == false
                                                                        ? x.LearnType == CourseLearnType.Selected
                                                                        : (x.LearnType == CourseLearnType.Must ||
                                                                           x.LearnType == CourseLearnType.MustAll))))
                                               on a.FromId equals b.Id
                                           group a by new { a.UserId, a.FromId }
                                       into g
                                           select new DepartmentImageUserOutputDto
                                           {
                                               UserId = g.Key.UserId,
                                               Number = g.Sum(x => x.Score)
                                           });
                           }
                           return null;
                       }
                       var userQuery = await GetQueryByUserIndexImage().ToListAsync();
                       var result = await (from a in _organizationUnitRepository.GetAll().Where(x => !x.IsDeleted)
                                           let number = (from b in _userOrganizationUnitRepository.GetAll()
                                               .Where(x => !x.IsDeleted && x.IsMain && x.OrganizationUnitId == a.Id)
                                                         join c in userQuery on b.UserId equals c.UserId
                                                         select c.Number)
                                           select new DepartmentImageOutputDto
                                           {
                                               DepartmentId = a.Id,
                                               DepartmentName = a.DisplayName,
                                               UserNumber = number.Count(),
                                               Number = number.Sum()
                                           }).ToListAsync();
                       if (input.Type == DepartmentStatisticType.AvgCourseScoreCount ||
                           input.Type == DepartmentStatisticType.AvgPersonCourseCount ||
                           input.Type == DepartmentStatisticType.AvgPersonCourseTimeCount)
                       {
                           var allperson = await GetRecommendedIndexImageByUser(new DepartmentImageInput()
                           {
                               IsMustLearn = input.IsMustLearn,
                               StatisticYear = input.StatisticYear,
                               StatisticMonth = input.StatisticMonth,
                               Type = DepartmentStatisticType.WatchPerson
                           }); //获取部门观看人数
                            result.ForEach(x =>
                           {
                                //获取当前部门当月观看人数
                                var myDepperson = allperson.FirstOrDefault(y => y.DepartmentId == x.DepartmentId);
                               double avgPerson = 0;
                               if (myDepperson?.UserNumber > 0)
                               {
                                   avgPerson = Convert.ToDouble(x.UserNumber) / Convert.ToDouble(myDepperson.UserNumber);
                               }
                               if (avgPerson > 1)
                               {
                                   avgPerson = 1; //如果观看人数少于此类型人数，则取整
                                }
                               x.Number = x.Number * avgPerson;
                           });
                       }
                       return result;
                   });
        }
        #endregion
        #region 课程统计

        /// <summary>
        /// 课程统计-推荐指标
        /// </summary>
        /// <returns></returns>
        public async Task<RecommendedIndexCourseOutputDto> GetRecommendedIndexByCourse()
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync("GetRecommendedIndexByCourse",
                async () =>
                {
                    var result = new RecommendedIndexCourseOutputDto
                    {
                        UpResource = await _courseRepository.CountAsync(x => !x.IsDeleted && x.Status == -1),
                        UpResourceTime = await _courseRepository.GetAll().Where(x => !x.IsDeleted && x.Status == -1)
                            .SumAsync(x => x.LearnTime),
                        LearnUser = await _userCourseRecordrepository.GetAll().Where(x => !x.IsDeleted)
                            .GroupBy(x => x.UserId).CountAsync(),
                        Experience = await _experienceRepository.GetAll().Where(x=>!x.IsDeleted && x.Type==TrainExperienceType.Course).CountAsync(),
                        Score = await _userCourseRecordrepository.GetAll().Where(x => !x.IsDeleted)
                            .SumAsync(x => x.Score ?? 0),
                        Comment = await _userCourseCommentRepository.CountAsync(x => !x.IsDeleted),
                        Favor = await _userCourseRecordrepository.GetAll()
                            .Where(x => !x.IsDeleted && x.IsFavor == true).CountAsync(),
                        Diss = await _userCourseRecordrepository.GetAll()
                            .Where(x => !x.IsDeleted && x.IsFavor == false).CountAsync()
                    };
                    var learnList = await _courseRecordDetailRepository.GetAll().Where(x => !x.IsDeleted)
                        .GroupBy(x => new { x.UserId, x.CourseId })
                        .Select(x => x.Max(y => y.LearningTime)).ToListAsync();
                    result.LearnTime = learnList.Sum();
                    return result;
                });
        }
        /// <summary>
        /// 获取分类统计图表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<CourseTypeImageOutputDto>> GetRecommendedIndexImageByCourse(CourseTypeImageInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync($"GetRecommendedIndexImageByCourse{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    var minTime = DateTime.Parse($"{input.StatisticYear}-{input.StatisticMonth}-01");
                    var maxTime = minTime.AddMonths(1);
                    var root = await _dictionRepository.GetAll()
                        .FirstOrDefaultAsync(x => !x.IsDeleted && x.Code.Equals(CourseDictionCode));
                    Func<IQueryable<CourseTypeImageOutputDto>> TypeQuery = () =>
                    {
                        switch (input.Type)
                        {
                            case CourseTypeStatisticType.UpResource:
                            case CourseTypeStatisticType.UpResourceTime:
                                return (from a in _courseRepository.GetAll().Where(x =>
                                        !x.IsDeleted && x.Status == -1 && x.CreationTime >= minTime &&
                                        x.CreationTime < maxTime)
                                    group a by a.CourseType
                                    into g
                                    select new CourseTypeImageOutputDto
                                    {
                                        CourseTypeId = g.Key,
                                        Number = input.Type == CourseTypeStatisticType.UpResource
                                            ? g.Count()
                                            : g.Sum(x => x.LearnTime)
                                    });
                            case CourseTypeStatisticType.Comment:
                                return (from b in _courseRepository.GetAll().Where(x => !x.IsDeleted)
                                    join a in _userCourseCommentRepository.GetAll().Where(x =>
                                            !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                        on b.Id equals a.CourseId
                                    group a by b.CourseType
                                    into g
                                    select new CourseTypeImageOutputDto
                                    {
                                        CourseTypeId = g.Key,
                                        Number = g.Count()
                                    });
                            case CourseTypeStatisticType.LearnTime:
                                return (from a in _courseRecordDetailRepository.GetAll().Where(x =>
                                        !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                    join b in _courseRepository.GetAll().Where(x => !x.IsDeleted) on a.CourseId
                                        equals b.Id
                                    group a by new { b.CourseType, a.UserId, a.CourseId }
                                    into g
                                    select new CourseTypeImageOutputDto
                                    {
                                        CourseTypeId = g.Key.CourseType,
                                        Number = g.Max(y => y.LearningTime)
                                    });
                            case CourseTypeStatisticType.Diss:
                            case CourseTypeStatisticType.Favor:
                                return (from a in _userCourseRecordrepository.GetAll().Where(x =>
                                        !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                    join b in _courseRepository.GetAll().Where(x => !x.IsDeleted) on a.CourseId equals b
                                        .Id
                                    group a by b.CourseType
                                    into g
                                    select new CourseTypeImageOutputDto
                                    {
                                        CourseTypeId = g.Key,
                                        Number = g.Count(x => x.IsFavor == (input.Type == CourseTypeStatisticType.Favor))
                                    });
                            case CourseTypeStatisticType.LearnUser:
                                return (from a in _userCourseRecordrepository.GetAll().Where(x =>
                                        !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                    join b in _courseRepository.GetAll().Where(x => !x.IsDeleted) on a.CourseId equals b
                                        .Id
                                    group a by new { b.CourseType, a.UserId }
                                    into g
                                    select new CourseTypeImageOutputDto
                                    {
                                        CourseTypeId = g.Key.CourseType,
                                        Number = 1
                                    });
                            case CourseTypeStatisticType.Experience:
                                return (from a in _experienceRepository.GetAll().Where(x=>!x.IsDeleted && x.Type == TrainExperienceType.Course
                                        && x.CreationTime >= minTime && x.CreationTime < maxTime)
                                        join b in _courseRepository.GetAll().Where(x => !x.IsDeleted) on a.TrainId equals b
                                            .Id
                                        group a by new { b.CourseType, a.UserId }
                                        into g
                                        select new CourseTypeImageOutputDto
                                        {
                                            CourseTypeId = g.Key.CourseType,
                                            Number = g.Count()
                                        });
                            case CourseTypeStatisticType.Score:
                                return (from a in _trainScoreRecordRepository.GetAll().Where(x =>
                                        !x.IsDeleted &&
                                        (x.FromType == TrainScoreFromType.CourseLearn ||
                                         x.FromType == TrainScoreFromType.CourseExperience ||
                                         x.FromType == TrainScoreFromType.CourseComment) && x.CreationTime >= minTime &&
                                        x.CreationTime < maxTime)
                                    join b in _courseRepository.GetAll().Where(x => !x.IsDeleted) on a.FromId equals b
                                        .Id
                                    group a by b.CourseType
                                    into g
                                    select new CourseTypeImageOutputDto
                                    {
                                        CourseTypeId = g.Key,
                                        Number = g.Sum(x => x.Score)
                                    });
                        }
                        return null;
                    };
                    var course = await TypeQuery().ToListAsync();
                    return await (from a in _dictionRepository.GetAll()
                            .Where(x => !x.IsDeleted && x.ParentID == root.Id)
                        let courseNumber = (from b in course
                            where b.CourseTypeId == a.Id
                            select (b.Number)).Sum()
                        select new CourseTypeImageOutputDto
                        {
                            CourseTypeId = a.Id,
                            CourseTypeName = a.Title,
                            Number = courseNumber
                        }).ToListAsync();
                });
        }

        /// <summary>
        /// 根据课程分类统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CourseTypeOutputDto>> GetCourseTypeStatistics(CourseTypeInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync(
                $"GetCourseTypeStatistics{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    var minTime = DateTime.Parse($"{input.StatisticYear}-{input.StatisticMonth}-01");
                    var maxTime = minTime.AddMonths(1);
                    var query = (from a in _dictionRepository.GetAll()
                            .Where(x => !x.IsDeleted && (input.CourseTypeId == null || x.Id == input.CourseTypeId))
                        join b in _courseRepository.GetAll().Where(x => !x.IsDeleted && (string.IsNullOrEmpty(input.CourseTypeName) || x.CourseName.Contains(input.CourseTypeName))) on a.Id equals b.CourseType
                                 let courseUser = (from c in _userCourseRecordrepository.GetAll().Where(x =>
                                         !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime &&
                                         x.CourseId == b.Id)
                                         select c.UserId).Distinct().Count()
                                 let courseUserTime = (from d in _userCourseRecordrepository.GetAll().Where(x =>
                                !x.IsDeleted && (x.LastModificationTime ?? x.CreationTime) >= minTime &&
                                (x.LastModificationTime ?? x.CreationTime) < maxTime &&
                                x.CourseId == b.Id)
                            select (d.LearnTime ?? 0)
                        ).Sum()
                        let experience=(from f in _experienceRepository.GetAll().Where(x=>!x.IsDeleted
                                        && x.CreationTime>=minTime && x.CreationTime<maxTime && x.TrainId==b.Id && x.Type == TrainExperienceType.Course)
                                        select f.Id
                                        ).Count()
                        let comment = (from e in _userCourseCommentRepository.GetAll().Where(x =>
                                !x.IsDeleted && (x.LastModificationTime ?? x.CreationTime) >= minTime &&
                                (x.LastModificationTime ?? x.CreationTime) < maxTime &&
                                x.CourseId == b.Id)
                            select e.Id
                        ).Count()
                        select new CourseTypeOutputDto
                        {
                            CourseId = b.Id,
                            Name = b.CourseName,
                            CourseUser = courseUser,
                            _CourseUserTime = courseUserTime,
                            Experience = experience,
                            Comment = comment
                        });
                    var toalCount = await query.CountAsync();
                    var ret = await query.OrderByDescending(r => r.Name).PageBy(input).ToListAsync();
                    var index = input.SkipCount + 1;
                    ret.ForEach(x =>
                    {
                        x.Index = index;
                        x.CourseUserTime = Math.Round(Convert.ToDouble(x._CourseUserTime) / 60, 2);
                        index++;
                    });
                    return new PagedResultDto<CourseTypeOutputDto>(toalCount, ret);
                });
        }

        public async Task<PagedResultDto<CourseUserOutputDto>> GetUserCourseStatistics(CourseUserInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync($"GetUserCourseStatistics{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    var minTime = DateTime.Parse($"{input.StatisticYear}-{input.StatisticMonth}-01");
                    var maxTime = minTime.AddMonths(1);
                    var query = (from b in _userCourseRecordrepository.GetAll().Where(x =>
                            !x.IsDeleted && x.CreationTime >= minTime && x.CreationTime < maxTime && input.CourseId == x.CourseId)
                        join a in UserManager.Users on b.UserId equals a.Id
                        join c in _courseRepository.GetAll().Where(x => !x.IsDeleted) on b.CourseId equals c.Id
                        let watchTime = (from d in _courseRecordNumberRepository.GetAll()
                                .Where(x => !x.IsDeleted && x.CourseId == c.Id && x.UserId == a.Id &&
                                            x.CreationTime >= minTime && x.CreationTime < maxTime)
                                .OrderByDescending(x => x.CreationTime)
                            select d.CreationTime)
                        select new CourseUserOutputDto
                        {
                            UserName = a.Name,
                            LearnState = b.IsComplete == null ? 0 : b.IsComplete == false ? 1 : 2,
                            StartTime = b.CreationTime,
                            LastWatchTime = watchTime.FirstOrDefault(),
                            WatchNumber = watchTime.Count()
                        });
                    var toalCount = await query.CountAsync();
                    var ret = await query.OrderByDescending(r => r.LearnState).PageBy(input).ToListAsync();
                    return new PagedResultDto<CourseUserOutputDto>(toalCount, ret);
                });
        }
        #endregion

        #region 独立统计

        public async Task<PagedResultDto<FrontEndCourseScoreOutputDto>> GetFrontEndCourseScoreStatistics(
            FrontEndCourseScoreInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync(
                $"GetFrontEndCourseScoreStatistics{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    if (input.EndTime != null)
                    {
                        input.EndTime = input.EndTime.Value.AddDays(1);
                    }
                    var query = (from a in UserManager.Users.Where(x =>
                            !x.IsDeleted &&
                            (string.IsNullOrEmpty(input.UserName) || x.Name.Contains(input.UserName)))
                        join b in _userOrganizationUnitRepository.GetAll().Where(x => !x.IsDeleted && x.IsMain) on a.Id
                            equals
                            b.UserId
                        join c in _organizationUnitRepository.GetAll().Where(x => !x.IsDeleted) on b.OrganizationUnitId
                            equals c.Id
                        let postname = (from d in _userPostsrepository.GetAll().Where(x =>
                                !x.IsDeleted && x.UserId == a.Id && x.OrgId == b.OrganizationUnitId)
                            join e in _postsRepository.GetAll().Where(x => !x.IsDeleted) on d.PostId equals e.Id
                            select
                                e.Name
                        ).FirstOrDefault()
                        let score = (from f in _trainScoreRecordRepository.GetAll().Where(x =>
                                !x.IsDeleted && (x.FromType == TrainScoreFromType.CourseLearn ||
                                                 x.FromType == TrainScoreFromType.CourseComment ||
                                                 x.FromType == TrainScoreFromType.CourseExperience)
                                && (input.StartTime == null || x.CreationTime >= input.StartTime)
                                && (input.EndTime == null || x.CreationTime < input.EndTime)
                                && x.UserId == a.Id)
                            select new
                            {
                                f.FromType,
                                f.Score
                            })
                        select new FrontEndCourseScoreOutputDto
                        {
                            UserName = a.Name,
                            Department = c.DisplayName,
                            Post = postname,
                            LearnScore =
                                score.Where(x => x.FromType == TrainScoreFromType.CourseLearn && x.Score > 0).Select(x => x.Score)
                                    .Sum(),
                            UnLearnScore =
                                score.Where(x => x.FromType == TrainScoreFromType.CourseLearn && x.Score < 0).Select(x => x.Score)
                                    .Sum(),
                            ExperienceScore = score.Where(x => x.FromType == TrainScoreFromType.CourseExperience)
                                .Select(x => x.Score).Sum(),
                            CommentScore = score.Where(x => x.FromType == TrainScoreFromType.CourseComment)
                                .Select(x => x.Score).Sum(),
                            AllScore = score.Select(x => x.Score).Sum(),
                        });
                    var toalCount = await query.CountAsync();
                    var ret = await query.OrderByDescending(r => r.AllScore).PageBy(input).ToListAsync();
                    return new PagedResultDto<FrontEndCourseScoreOutputDto>(toalCount, ret);
                });
        }

        #endregion

    }
}
