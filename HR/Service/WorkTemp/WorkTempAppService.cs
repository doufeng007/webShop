using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.File;
using Abp.UI;
using ZCYX.FRMSCore.Application;
using Abp.Extensions;
using ZCYX.FRMSCore;
using Abp;
using ZCYX.FRMSCore.Extensions;
using Abp.Domain.Uow;
using ZCYX.FRMSCore.Model;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using Microsoft.AspNetCore.Hosting;
using ZCYX.FRMSCore.Users;

namespace HR
{
    public class WorkTempAppService : FRMSCoreAppServiceBase, IWorkTempAppService
    {
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<PostInfo, Guid> _postInfoRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ProjectAudit, Guid> _projectAuditRepository;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IConfigurationRoot _appConfiguration;

        public WorkTempAppService(WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IRepository<UserPosts, Guid> userPostRepository, IRepository<PostInfo, Guid> postInfoRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository
            , IUnitOfWorkManager unitOfWorkManager, IRepository<ProjectAudit, Guid> projectAuditRepository, ProjectAuditManager projectAuditManager
            , WorkFlowCacheManager workFlowCacheManager, IWorkFlowTaskRepository workFlowTaskRepository, IDynamicRepository dynamicRepository, IHostingEnvironment env)
        {
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _userPostRepository = userPostRepository;
            _postInfoRepository = postInfoRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _projectAuditRepository = projectAuditRepository;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _dynamicRepository = dynamicRepository;

            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
           
        }


        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<WorkTempListOutputDto>> GetList(GetWorkTempListInput input)
        {
            var isAll = false;
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            if (userRoles.Any(r => r == "XZRY" || r == "ZJL"))
            {
                isAll = true;
            }
            var workonFlowId = new Guid(_appConfiguration["WorkTemp:Workon"]);
            var workoutFlowId = new Guid(_appConfiguration["WorkTemp:Workout"]);
            var askForLeaveFlowId = new Guid(_appConfiguration["WorkTemp:EmployeeAskForLeave"]);
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var ret = await _dynamicRepository.QueryAsync<WorkTemp>($"exec [dbo].[spWorkTemp]");
                var query = from a in ret
                            join u in UserManager.Users on a.CreatorUserId equals u.Id
                            let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                              ((x.FlowID == workoutFlowId && a.Type==Enum.WorkTempType.出差) || (x.FlowID == workonFlowId && a.Type == Enum.WorkTempType.加班)|| (x.FlowID == askForLeaveFlowId && a.Type == Enum.WorkTempType.请假)) && x.InstanceID == a.Id.ToString() &&
                                                              x.ReceiveID == AbpSession.UserId.Value)
                                             select c)
                            where  (isAll || (a.CreatorUserId == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "")))                      
                            select new WorkTempListOutputDto
                            {
                                Id = a.Id,
                                CreatorUserId = a.CreatorUserId,
                                Type = a.Type,
                                TypeName = a.Type.ToString(),
                                Name = u.Name,
                                Code = u.WorkNumber,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                                Hours = a.Hours,
                                OrgId = a.OrgId,
                                Status = a.Status,
                                CreationTime = a.CreationTime,
                                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,

                            };
                if (input.Type.HasValue)
                    query = query.Where(x => x.Type == input.Type);
                if (input.OrgId.HasValue)
                    query = query.Where(x => x.OrgId == input.OrgId);

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
                if (!string.IsNullOrEmpty(input.SearchKey))
                {
                    query = query.Where(r => r.Name.Contains(input.SearchKey));
                }
                var toalCount =  query.Count();
                input.MaxResultCount = input.MaxResultCount == 0 ? 10 : input.MaxResultCount;
                var data = query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).Skip( input.SkipCount).Take(input.MaxResultCount).ToList();

                foreach (var item in data)
                {
                    item.InstanceId = item.Id.ToString();
                    switch (item.Type)
                    {
                        case Enum.WorkTempType.请假:
                            input.FlowId = askForLeaveFlowId;
                            break;
                        case Enum.WorkTempType.出差:
                            input.FlowId = workoutFlowId;
                            break;
                        case Enum.WorkTempType.加班:
                            input.FlowId = workonFlowId;
                            break;
                    }
                    var o = from b in _userOrganizationUnitRepository.GetAll().Where(x=>!x.IsDeleted)
                            join c in _organizationUnitRepository.GetAll().Where(x => !x.IsDeleted) on b.OrganizationUnitId equals c.Id
                            where b.UserId == item.CreatorUserId
                            select c.DisplayName;
                    item.DepartmentName = string.Join(',', o.Distinct().ToList());
                    var posts = from a in _userPostRepository.GetAll().Where(x => !x.IsDeleted)
                                join b in _postInfoRepository.GetAll().Where(x => !x.IsDeleted) on a.PostId equals b.Id
                                join c in _organizationUnitRepository.GetAll().Where(x => !x.IsDeleted) on a.OrgId equals c.Id
                                where a.UserId == item.CreatorUserId
                                select new { DisplayName = b.Name+(a.IsMain?"(主)":"") };
                    item.Posts= string.Join(',', posts.Select(x=>x.DisplayName).ToList());
                    _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                }
                return new PagedResultDto<WorkTempListOutputDto>(toalCount, data);
            }

        }
    
    }
}
