using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Users;

namespace Project.Service.OA.Workout
{
    public class OAWorkoutAppService : FRMSCoreAppServiceBase, IOAWorkoutAppService
    {
        public readonly IRepository<OAWorkout, Guid> _oaWorkoutRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<PostInfo, Guid> _postInfoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private RoleRelationManager _roleRelationManager;

        public OAWorkoutAppService(IRepository<OAWorkout, Guid> oaWorkoutRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IRepository<PostInfo, Guid> postInfoRepository, IRepository<UserPosts, Guid> userPostRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , IUnitOfWorkManager unitOfWorkManager, IRepository<AbpDictionary, Guid> abpDictionaryRepository
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IWorkFlowTaskRepository workFlowTaskRepository, RoleRelationManager roleRelationManager, IRepository<User, long> userRepository)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaWorkoutRepository = oaWorkoutRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _postInfoRepository = postInfoRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userPostRepository = userPostRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpDictionaryRepository = abpDictionaryRepository;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _roleRelationManager = roleRelationManager;
            _userRepository = userRepository;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        [AbpAuthorize]
        public async Task<InitWorkFlowOutput> Create(OAWorkoutInputDto input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var model = input.MapTo<OAWorkout>();
            var hours = (model.EndTime.Value - model.StartTime.Value).Hours;
            if (hours < 0)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不能晚于结束时间。");
            }
            if (input.RelationUserId.HasValue)
            {
                if (_roleRelationManager.IsExistence(input.RelationUserId.Value, input.StartTime.Value, input.EndTime.Value))
                    throw new UserFriendlyException("委托人已经被委托了。");
            }
            model.Status = 0;
            model.OrgId = userOrgModel.OrgId;
            model.RelationUserId = input.RelationUserId;
            model.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            var ret = _oaWorkoutRepository.Insert(model);

            return new InitWorkFlowOutput() { InStanceId = ret.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAWorkoutDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaWorkoutRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAWorkoutDto>();

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var orgModel = await _organizationUnitRepository.GetAsync(ret.OrgId);
                model.DepartmentName = orgModel.DisplayName;

                if (!ret.PostIds.IsNullOrWhiteSpace())
                {
                    var postIds = ret.PostIds.Split(',');
                    var postModels = await _postInfoRepository.GetAll().Where(r => postIds.Contains(r.Id.ToString())).ToListAsync();
                    if (postModels.Count() > 0)
                        model.Post_Name = string.Join("、", postModels.Select(r => r.Name));
                }
                if (ret.CreatorUserId.HasValue)
                    model.UserId_Name = (await UserManager.GetUserByIdAsync(ret.CreatorUserId.Value)).Name;
                if (ret.RelationUserId.HasValue)
                    model.RelationUserName = (await UserManager.GetUserByIdAsync(ret.RelationUserId.Value)).Name;

            }

            return model;
        }
        public List<OAWorkoutListByCarDto> GetList(GetOAWorkoutListByCarInput input)
        {


            var ret = from a in _oaWorkoutRepository.GetAll().Where(x => x.Status == -1).WhereIf(input.IsOld && input.List.Count > 0, y => input.List.Contains(y.Id)).WhereIf(input.UserId.HasValue, x => x.CreatorUserId == input.UserId.Value)
                      join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                      where !a.IsDeleted
                      select new OAWorkoutListByCarDto
                      {
                          Id = a.Id,
                          UserId = a.CreatorUserId.Value,
                          UserName = b.Name,
                          Address = a.Destination,
                          StartTime = a.StartTime,
                          EndTime = a.EndTime
                      };
            return ret.ToList();
        }
        [AbpAuthorize]
        public PagedResultDto<OAWorkoutListDto> GetAll(GetOAWorkoutListInput input)
        {

            //var query1 = from a in UserManager.Users
            //             join b in _userPostRepository.GetAll() on a.Id equals b.UserId into g
            //             from gg in g.DefaultIfEmpty()
            //             where a.Id == 50045
            //             select new { a, gg };
            //var ret1 = query1.ToList();

            //var query2 = UserManager.Users.Where(r => r.Id == 50045).GroupJoin(_userPostRepository.GetAll(), a => a.Id, b => b.UserId, (a, b) => new { a, b }).Select(o => o);
            //var ret2 = query2.ToList();


            //var query3 = from a in UserManager.Users
            //             join b in _userPostRepository.GetAll() on a.Id equals b.UserId into g
            //             where a.Id == 50045
            //             select new { a, g };

            //var ret3 = query3.ToList();




            //var query4 = from a in UserManager.Users
            //             join b in _userPostRepository.GetAll() on a.Id equals b.UserId into g
            //             where a.Id == 50045
            //             select new
            //             {
            //                 a,
            //                 p = from p in g
            //                     join o in _postInfoRepository.GetAll() on p.PostId equals o.Id
            //                     select new { PostId = o.Id, PostName = o.Name },
            //             };

            //var ret4 = query4.ToList();
            var isAll = false;
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            if (userRoles.Any(r => string.Compare(r, "XZRY", true) == 0 || string.Compare(r, "ZJL", true) == 0))
            {
                isAll = true;
            }
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {

                var query = from a in _oaWorkoutRepository.GetAll()
                            join u in UserManager.Users on a.CreatorUserId.Value equals u.Id
                            join m in _organizationUnitRepository.GetAll() on a.OrgId equals m.Id
                            join b in UserManager.Users on a.RelationUserId equals b.Id into tmp1
                            from c in tmp1.DefaultIfEmpty()
                            let cc = (from o in _postInfoRepository.GetAll()
                                      where a.PostIds.GetStrContainsArray(o.Id.ToString())
                                      select new { PostId = o.Id, PostName = o.Name }).ToList()
                            let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                    x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                    x.ReceiveID == AbpSession.UserId.Value)
                                             select c)
                            where !a.IsDeleted && (isAll || (a.CreatorUserId.Value == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "")))
                            select new
                            {
                                Id = a.Id,
                                Title = a.Title,
                                CreationTime = a.CreationTime,
                                CreatorUserId = a.CreatorUserId,
                                RelationUserId = a.RelationUserId,
                                RelationUserName = c != null ? c.Name : "",
                                UserId_Name = u.Name,
                                BeginTime = a.StartTime,
                                EndTime = a.EndTime,
                                Hour = a.Hours,
                                Destination = a.Destination,
                                OrgId = a.OrgId,
                                Reason = a.Reason,
                                Status = a.Status,
                                //TenantId = a.TenantId,
                                OrgName = m.DisplayName,
                                Post = cc,
                                FromPosition = a.FromPosition,
                                TranType = a.TranType,
                                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                            };

                if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
                {
                    query = query.Where(ite => ite.UserId_Name.Contains(input.SearchKey));
                }

                if (input.GetMy)
                {
                    query = query.Where(r => r.CreatorUserId == AbpSession.UserId.Value);
                }
                else
                {
                    if (isAll)  //总经理或 行政人员看
                    {
                        query = query.Where(r => r.Status > 0);
                    }
                }
                if (!input.Status.IsNullOrWhiteSpace())
                {
                    var statusArry = input.Status.Split(',').ToList();
                    query = query.Where(r => statusArry.Contains(r.Status == null ? "" : r.Status.Value.ToString()));
                }


                var count = query.Count();
                var list = query.OrderBy(r => r.OpenModel).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToList();
                var ret = new List<OAWorkoutListDto>();
                foreach (var item in list)
                {
                    var entity = new OAWorkoutListDto()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        StartTime = item.BeginTime,
                        RelationUserId = item.RelationUserId,
                        CreationTime = item.CreationTime,
                        EndTime = item.EndTime,
                        DepartmentName = item.OrgName,
                        Destination = item.Destination,
                        Reason = item.Reason,
                        //Remark = item.Remark,
                        Status = item.Status.HasValue ? item.Status.Value : 0,
                        UserId = item.CreatorUserId.Value,
                        UserId_Name = item.UserId_Name,
                        //TenantId = item.TenantId,
                        OrgId = item.OrgId,
                        Hours = item.Hour,
                        FromPosition = item.FromPosition,
                        TranType = item.TranType,

                    };

                    if (item.Post == null || item.Post.Count() == 0)
                    {

                    }
                    else
                    {
                        var potsList = item.Post.Select(r => r.PostName);
                        entity.PostName = string.Join(",", potsList);
                    }


                    entity.InstanceId = item.Id.ToString();
                    _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, entity as BusinessWorkFlowListOutput);
                    ret.Add(entity);
                }
                return new PagedResultDto<OAWorkoutListDto>(count, ret);
            }
        }

        public async Task Update(OAWorkoutInputDto input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var hours = (input.EndTime.Value - input.StartTime.Value).Hours;
            if (hours < 0)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不能晚于结束时间。");
            }
            var ret = _oaWorkoutRepository.Get(input.Id.Value);
            var old_Model = ret.DeepClone();
            ret = input.MapTo(ret);
            ret.OrgId = userOrgModel.OrgId;
            ret.RelationUserId = input.RelationUserId;
            ret.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            _oaWorkoutRepository.Update(ret);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(ret));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }


        }

        private OAWorkoutChangeDto GetChangeModel(OAWorkout model)
        {
            /// 如果有外键数据 在这里转换
            var ret = new OAWorkoutChangeDto();
            ret.StartTime = model.StartTime;
            ret.EndTime = model.EndTime;
            ret.Reason = model.Reason;
            ret.Hours = model.Hours;
            ret.FromPosition = model.FromPosition;
            ret.Destination = model.Destination;
            ret.IsCar = model.IsCar.HasValue && model.IsCar.Value ? "需要" : "";

            if (model.RelationUserId.HasValue)
            {
                var user = UserManager.Users.FirstOrDefault(x => x.Id == model.RelationUserId.Value);
                if (user != null)
                    ret.RelationUserId_Name = user.Name;
            }
            var tranTypeModel = _abpDictionaryRepository.GetAll().FirstOrDefault(r => r.Id == model.TranType);
            if (tranTypeModel != null)
                ret.TranType_Name = tranTypeModel.Title;
            return ret;
        }
        public void WorkoutRelationUserId(Guid instanceID)
        {
            var model = _oaWorkoutRepository.Get(instanceID);
            if (model.Status == -1 && model.RelationUserId.HasValue)
            {
                var input = new CreateRoleRelationInput()
                {
                    UserId = model.CreatorUserId.Value,
                    RelationUserId = model.RelationUserId.Value,
                    StartTime = model.StartTime.Value,
                    Type = RelationType.Workout,
                    RelationId = model.Id,
                    EndTime = model.EndTime.Value
                };
                _roleRelationManager.Create(input);
            }
        }
    }
}
