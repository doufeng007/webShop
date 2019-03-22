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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Users;

namespace HR
{
    public class OAWorkonAppService : FRMSCoreAppServiceBase, IOAWorkonAppService
    {
        public readonly IRepository<OAWorkon, Guid> _oaWorkonRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<PostInfo, Guid> _postInfoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;

        public OAWorkonAppService(IRepository<OAWorkon, Guid> oaWorkonRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IRepository<PostInfo, Guid> postInfoRepository, IRepository<UserPosts, Guid> userPostRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , IUnitOfWorkManager unitOfWorkManager, ProjectAuditManager projectAuditManager
            , WorkFlowCacheManager workFlowCacheManager, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            _oaWorkonRepository = oaWorkonRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _postInfoRepository = postInfoRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userPostRepository = userPostRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        [AbpAuthorize]
        public async Task<InitWorkFlowOutput> Create(OAWorkonInputDto input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var model = input.MapTo<OAWorkon>();
            model.Status = 0;
            model.OrgId = userOrgModel.OrgId;
            model.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            var ret = _oaWorkonRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = ret.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAWorkonDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaWorkonRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAWorkonDto>();
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
            }
            return model;
        }


        [AbpAuthorize]
        public PagedResultDto<OAWorkonListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var isAll = false;
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            if (userRoles.Any(r => string.Compare(r, "XZRY", true) == 0 || string.Compare(r, "ZJL", true) == 0))
            {
                isAll = true;
            }
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _oaWorkonRepository.GetAll()
                            join u in UserManager.Users on a.CreatorUserId.Value equals u.Id
                            join m in _organizationUnitRepository.GetAll() on a.OrgId equals m.Id
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
                                UserId_Name = u.Name,
                                BeginTime = a.StartTime,
                                EndTime = a.EndTime,
                                Hour = a.Hours,
                                OrgId = a.OrgId,
                                Reason = a.Reason,
                                Status = a.Status,
                                //TenantId = a.TenantId,
                                OrgName = m.DisplayName,
                                Post = cc,
                                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,

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

                var count = query.Count();
                var list = query.OrderBy(r => r.OpenModel).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToList();
                var ret = new List<OAWorkonListDto>();
                foreach (var item in list)
                {
                    var entity = new OAWorkonListDto()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        StartTime = item.BeginTime,
                        CreationTime = item.CreationTime,
                        EndTime = item.EndTime,
                        DepartmentName = item.OrgName,
                        Reason = item.Reason,
                        //Remark = item.Remark,
                        Status = item.Status.HasValue ? item.Status.Value : 0,
                        UserId = item.CreatorUserId.Value,
                        UserId_Name = item.UserId_Name,
                        //TenantId = item.TenantId,
                        OrgId = item.OrgId,
                        Hours = item.Hour,
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
                return new PagedResultDto<OAWorkonListDto>(count, ret);
            }
        }

        public async Task Update(OAWorkonInputDto input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var ret = _oaWorkonRepository.Get(input.Id.Value);
            var old_Model = ret.DeepClone();
            ret = input.MapTo(ret);
            ret.OrgId = userOrgModel.OrgId;
            ret.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            _oaWorkonRepository.Update(ret);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(ret));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }


        private OAWorkonChangeDto GetChangeModel(OAWorkon model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<OAWorkonChangeDto>();
            return ret;
        }
    }
}
