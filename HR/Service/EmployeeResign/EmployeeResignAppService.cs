using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.WorkFlow;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Users;

namespace HR
{
    [AbpAuthorize]
    public class EmployeeResignAppService : FRMSCoreAppServiceBase, IEmployeeResignAppService
    {
        private readonly IRepository<EmployeeResign, Guid> _employeeResignRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<UserPosts, Guid> _userPostsRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;

        public EmployeeResignAppService(IRepository<EmployeeResign, Guid> employeeResignRepository, IWorkFlowTaskRepository workFlowTaskRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager, WorkFlowCacheManager workFlowCacheManager,
            IRepository<User, long> usersRepository, ProjectAuditManager projectAuditManager,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<PostInfo, Guid> postsRepository, IRepository<UserPosts, Guid> userPostsRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository, IUnitOfWorkManager unitOfWorkManager
            )
        {
            _employeeResignRepository = employeeResignRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _postsRepository = postsRepository;
            _userPostsRepository = userPostsRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _usersRepository = usersRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _workFlowCacheManager = workFlowCacheManager;
            _projectAuditManager = projectAuditManager;
        }

        /// <summary>
        /// 创建员工离职申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateEmployeeResignInput input)
        {
            var model = new EmployeeResign();
            var has = _employeeResignRepository.FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value && ite.Status > 0);
            if (has != null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "有未完成的离职申请，请等待处理完成后再创建新申请。");
            }

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            model.Type = input.Type;
            model.Status = 0;
            model.Reason = input.Reason;
            model.UserId = AbpSession.UserId.Value;
            model.OrgId = userOrgModel.OrgId;
            model.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            _employeeResignRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        /// <summary>
        /// 获取离职申请详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<EmployeeResignDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var r = await _employeeResignRepository.GetAsync(id);
            var user = _usersRepository.Get(r.UserId);
            var tmp = new EmployeeResignDto();

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var orgModel = await _organizationUnitRepository.GetAsync(r.OrgId);
                tmp.DepartmentName = orgModel.DisplayName;

                if (!r.PostIds.IsNullOrWhiteSpace())
                {
                    var postIds = r.PostIds.Split(',');
                    var postModels = await _postsRepository.GetAll().Where(x => postIds.Contains(x.Id.ToString())).ToListAsync();
                    if (postModels.Count() > 0)
                        tmp.PostName = string.Join("、", postModels.Select(x => x.Name));
                }
            }
            tmp.CreationTime = r.CreationTime;
            tmp.Id = r.Id;
            tmp.Name = user.Name;
            tmp.WorkNumber = user.WorkNumber;
            tmp.Type = r.Type;
            tmp.Type_Name = r.Type.ToString();
            tmp.Reason = r.Reason;
            tmp.Status = r.Status;
            tmp.StatusTitle = r.Status.ToString();
            return tmp;
        }

        /// <summary>
        /// 获取草稿状态的离职申请信息，可用于编辑
        /// </summary>
        /// <returns></returns>
        public async Task<EmployeeResignDto> GetDraft()
        {
           
            var r = await _employeeResignRepository.GetAll().FirstOrDefaultAsync(ite=>ite.CreatorUserId==AbpSession.UserId.Value&&ite.Status==0);
            if (r == null) {
                return null;
            }
            var user = _usersRepository.Get(r.UserId);
            var tmp = new EmployeeResignDto();

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var orgModel = await _organizationUnitRepository.GetAsync(r.OrgId);
                tmp.DepartmentName = orgModel.DisplayName;

                if (!r.PostIds.IsNullOrWhiteSpace())
                {
                    var postIds = r.PostIds.Split(',');
                    var postModels = await _postsRepository.GetAll().Where(x => postIds.Contains(x.Id.ToString())).ToListAsync();
                    if (postModels.Count() > 0)
                        tmp.PostName = string.Join("、", postModels.Select(x => x.Name));
                }
            }
            tmp.CreationTime = r.CreationTime;
            tmp.Id = r.Id;
            tmp.Name = user.Name;
            tmp.WorkNumber = user.WorkNumber;
            tmp.Type = r.Type;
            tmp.Type_Name = r.Type.ToString();
            tmp.Reason = r.Reason;
            tmp.Status = r.Status;
            tmp.StatusTitle = r.Status.ToString();
            return tmp;
        }
        /// <summary>
        /// 获取离职申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeResignListDto>> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var query = from a in _employeeResignRepository.GetAll()
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new EmployeeResignListDto() {
                             CreationTime=a.CreationTime,
                              CreatorUserId=a.CreatorUserId,
                              UserId =a.UserId,
                            Reason = a.Reason,
                              OrgId= a.OrgId,
                              Type= a.Type,
                               Id=a.Id,
                              PostIds= a.PostIds,
                              Status=(int)a.Status,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };

            //var query = _employeeResignRepository.GetAll().Where(ite => ite.UserId == AbpSession.UserId.Value || ite.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : ""));
            if (input.GetMy == true)
            {
                query = query.Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            }
            var totalCount = query.Count();

            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync());
            
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (ret.Count > 0)
                {
                    foreach (var tmp in ret)
                    {
                        var user = _usersRepository.Get(tmp.UserId);
                        
                        var orgModel = await _organizationUnitRepository.GetAsync(tmp.OrgId);
                        tmp.DepartmentName = orgModel.DisplayName;

                        if (!tmp.PostIds.IsNullOrWhiteSpace())
                        {
                            var postIds = tmp.PostIds.Split(',');
                            var postModels = await _postsRepository.GetAll().Where(x => postIds.Contains(x.Id.ToString())).ToListAsync();
                            if (postModels.Count() > 0)
                                tmp.PostName = string.Join("、", postModels.Select(x => x.Name));
                        }
                        tmp.Name = user.Name;
                        tmp.WorkNumber = user.WorkNumber;                      
                        tmp.Type_Name = tmp.Type.ToString();
                        tmp.InstanceId = tmp.Id.ToString();
                        _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, tmp as BusinessWorkFlowListOutput);
                    }
                }
            }
            return new PagedResultDto<EmployeeResignListDto>(totalCount, ret);
        }
        /// <summary>
        /// 更新离职申请
        /// </summary>
        /// <param name="input"></param>
        public async Task Update(UpdateEmployeeResignInput input)
        {
            var model = _employeeResignRepository.Get(input.Id);
            var old_Model = model.DeepClone();
            model.Reason = input.Reason;
            model.Type = input.Type;
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });

            model.OrgId = userOrgModel.OrgId;
            model.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            _employeeResignRepository.Update(model);
            if (input.IsUpdateForChange) {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }

        private UpdateEmployeeResignLogDto GetChangeModel(EmployeeResign model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<UpdateEmployeeResignLogDto>();
            ret.Type = model.Type.ToString();
            return ret;
        }
        public bool EmployeeResignSuccess(Guid guid)
        {
            var model = _employeeResignRepository.GetAll().FirstOrDefault(x => x.Id == guid);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            var user = _usersRepository.GetAll().FirstOrDefault(x => x.Id == model.UserId);
            if (user == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用户数据不存在！");
            if (user.IsActive)
            {
                var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
            .Resolve<IUserAppService>();
                _service.Enable(new EntityDto<long>(model.UserId));
                return true;
            }
            return false;
        }

        public bool EmployeeResignIsLeader(Guid guid)
        {
            var model = _employeeResignRepository.GetAll().FirstOrDefault(x => x.Id == guid);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            var user = _usersRepository.GetAll().FirstOrDefault(x => x.Id == model.UserId);
            if (user == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用户数据不存在！");
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            return manager.IsChargerLeaderOrDivision(user.Id);
        }
    }
}
