using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.File;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using HR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Train.Enum;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Extensions;

namespace Train
{
    public class TrainStatisticsAppService : FRMSCoreAppServiceBase, ITrainStatisticsAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserTrainScoreRecord, Guid> _trainScoreRecordRepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<UserPosts, Guid> _userPostsrepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public static string StatisiticCacheName = "TrainStatistics";
        public TrainStatisticsAppService(IRepository<UserTrainScoreRecord, Guid> trainScoreRecordRepository, IRepository<User, long> userRepository,
            IRepository<PostInfo, Guid> postsRepository, IRepository<UserPosts, Guid> userPostsRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IUnitOfWorkManager unitOfWorkManager, ICacheManager cacheManager, IRepository<UserPosts, Guid> userPostsrepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository)
        {
            _trainScoreRecordRepository = trainScoreRecordRepository;
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _postsRepository = postsRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _cacheManager = cacheManager;
            _userPostsrepository = userPostsrepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }


        public async Task<PagedResultDto<TrainScoreOutputDto>> GetList(
          TrainScoreInput input)
        {
            return await _cacheManager.GetCache(StatisiticCacheName).GetAsync(
                $"GetTrainScoreStatistics{JsonConvert.SerializeObject(input).GetMd5()}",
                async () =>
                {
                    if (input.EndTime != null)
                    {
                        input.EndTime = input.EndTime.Value.AddDays(1);
                    }
                    var query = (from a in UserManager.Users.Where(x =>
                            !x.IsDeleted &&
                            (string.IsNullOrEmpty(input.SearchKey) || x.Name.Contains(input.SearchKey)))
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
                                         !x.IsDeleted && (x.FromType == TrainScoreFromType.TrainLearn || x.FromType == TrainScoreFromType.TrainExperience)
                                         && (input.StartTime == null || x.CreationTime >= input.StartTime)
                                         && (input.EndTime == null || x.CreationTime < input.EndTime)
                                         && x.UserId == a.Id)
                                              select new
                                              {
                                                  f.FromType,
                                                  f.Score
                                              })
                                 select new TrainScoreOutputDto
                                 {
                                     Name = a.Name,
                                     DepartmentName = c.DisplayName,
                                     PostName = postname,
                                     JoinScore =
                                         score.Where(x => x.FromType == TrainScoreFromType.TrainLearn).Select(x => x.Score)
                                             .Sum(),
                                     ExperienceScore = score.Where(x => x.FromType == TrainScoreFromType.TrainExperience)
                                         .Select(x => x.Score).Sum(),
                                     Score = score.Select(x => x.Score).Sum()
                                 });
                    var toalCount = await query.CountAsync();
                    var ret = await query.OrderByDescending(r => r.Score).PageBy(input).ToListAsync();
                    return new PagedResultDto<TrainScoreOutputDto>(toalCount, ret);
                });
        }
        //public async Task<PagedResultDto<TrainScoreOutputDto>> GetList(TrainScoreInput input)
        //{
        //    var query = from a in _userTrainScoreRecordRepository.GetAll().Where(x => !x.IsDeleted && (x.FromType == TrainScoreFromType.TrainExperience || x.FromType == TrainScoreFromType.TrainLearn)) select a;
        //    if (input.StartTime != null)
        //    {
        //        query = query.Where(x => x.CreationTime >= input.StartTime);
        //    }
        //    if (input.EndTime != null)
        //    {
        //        query = query.Where(x => x.CreationTime <= input.EndTime);
        //    }
        //    if (!string.IsNullOrEmpty(input.SearchKey))
        //    {
        //        query = from a in query
        //                where _userRepository.GetAll().Count(x => x.Name.Contains(input.SearchKey) && x.Id == a.UserId) > 0
        //                select a;
        //    }
        //   var retData = query;
        //   var ret = query.GroupBy(x => x.UserId);

        //    var toalCount = await ret.CountAsync();
        //    var data = await ret.OrderByDescending(r => r.Key).PageBy(input).ToListAsync();
        //    var list = new List<TrainScoreOutputDto>();
        //    foreach (var item in data)
        //    {
        //        var user = _userRepository.GetAll().First(x=>x.Id==item.Key);
        //        var tsmodel = new TrainScoreOutputDto();
        //        tsmodel.Name = user.Name;
        //        var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
        //        var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id =user.Id, }, new NullableIdDto<long>() { Id = null });
        //        tsmodel.DepartmentName = userOrgModel.OrgId_Name;
        //        tsmodel.PostName = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostName));
        //        tsmodel.JoinScore = retData.Where(x => x.UserId == item.Key&& x.FromType == TrainScoreFromType.TrainLearn).Sum(x => x.Score);
        //        tsmodel.ExperienceScore = retData.Where(x => x.UserId == item.Key&& x.FromType == TrainScoreFromType.TrainExperience).Sum(x => x.Score);
        //        tsmodel.Score = tsmodel.JoinScore + tsmodel.ExperienceScore;
        //        list.Add(tsmodel);
        //    }
        //    return new PagedResultDto<TrainScoreOutputDto>(toalCount, list);

        //}

    }
}
