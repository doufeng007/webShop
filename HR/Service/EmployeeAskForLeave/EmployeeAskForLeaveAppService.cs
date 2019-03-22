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
using ZCYX.FRMSCore.Users;

namespace HR
{
    public class EmployeeAskForLeaveAppService : FRMSCoreAppServiceBase, IEmployeeAskForLeaveAppService
    {
        private readonly IRepository<EmployeeAskForLeave, Guid> _repository;
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
        private RoleRelationManager _roleRelationManager;
        public EmployeeAskForLeaveAppService(IRepository<EmployeeAskForLeave, Guid> repository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IRepository<UserPosts, Guid> userPostRepository, IRepository<PostInfo, Guid> postInfoRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository
            , IUnitOfWorkManager unitOfWorkManager, IRepository<ProjectAudit, Guid> projectAuditRepository, ProjectAuditManager projectAuditManager
            , WorkFlowCacheManager workFlowCacheManager, IWorkFlowTaskRepository workFlowTaskRepository, RoleRelationManager roleRelationManager)
        {
            this._repository = repository;
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
            _roleRelationManager = roleRelationManager;
        }




        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<EmployeeAskForLeaveOutputDto> GetEmployeeAskForLeave(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.GetAsync(id);
            var ret = model.MapTo<EmployeeAskForLeaveOutputDto>();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var orgModel = await _organizationUnitRepository.GetAsync(model.OrgId);
                ret.DepartmentName = orgModel.DisplayName;

                if (!ret.PostIds.IsNullOrWhiteSpace())
                {
                    var postIds = ret.PostIds.Split(',');
                    var postModels = await _postInfoRepository.GetAll().Where(r => postIds.Contains(r.Id.ToString())).ToListAsync();
                    if (postModels.Count() > 0)
                        ret.Post_Name = string.Join("、", postModels.Select(r => r.Name));
                }
                ret.UserId = model.UserId;
                ret.UserId_Name = (await UserManager.GetUserByIdAsync(model.UserId)).Name;
                if(ret.RelationUserId.HasValue)
                    ret.RelationUseName = (await UserManager.GetUserByIdAsync(ret.RelationUserId.Value)).Name;
            }

            //var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            //var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = model.UserId, }, new NullableIdDto<long>() { Id = null });
            //ret.DepartmentName = userOrgModel.OrgId_Name;
            //ret.Post_Name = string.Join("、", userOrgModel.UserPosts.Select(r => r.PostName));

            return ret;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<EmployeeAskForLeaveListOutputDto>> GetEmployeeAskForLeaveList(GetEmployeeAskForLeaveListInput input)
        {
            var isAll = false;
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            if (userRoles.Any(r => r == "XZRY" || r == "ZJL"))
            {
                isAll = true;
            }
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll()
                            join u in UserManager.Users on a.UserId equals u.Id
                            join b in UserManager.Users on a.RelationUserId equals b.Id into tmp1
                            from c in tmp1.DefaultIfEmpty()
                            join m in _organizationUnitRepository.GetAll() on a.OrgId equals m.Id
                            let cc = (from o in _postInfoRepository.GetAll()
                                      where a.PostIds.GetStrContainsArray(o.Id.ToString())
                                      select new { PostId = o.Id, PostName = o.Name }).ToList()
                            let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                              x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                              x.ReceiveID == AbpSession.UserId.Value)
                                             select c)
                            //let cureentSteps = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString()
                            //                    && (x.Status == 1 || x.Status == 2))
                            //                    select c)
                            where !a.IsDeleted && (isAll || (a.CreatorUserId.Value == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "")))
                            select new
                            {
                                Id = a.Id,
                                UserId = a.UserId,
                                RelationUserId = a.RelationUserId,
                                RelationUserName = c != null ? c.Name : "",
                                UserId_Name = u.Name,
                                BeginTime = a.BeginTime,
                                EndTime = a.EndTime,
                                Hour = a.Hours,
                                OrgId = a.OrgId,
                                Reason = a.Reason,
                                Status = a.Status,
                                Remark = a.Remark,
                                TenantId = a.TenantId,
                                OrgName = m.DisplayName,
                                Post = cc,
                                CreationTime = a.CreationTime,
                                CreatorUserId = a.CreatorUserId,
                                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,

                            };
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
                if (!input.SearchKey.IsNullOrWhiteSpace())
                {
                    query = query.Where(r => r.UserId_Name.Contains(input.SearchKey));
                }
                if (!input.Status.IsNullOrWhiteSpace())
                {
                    var statusArry = input.Status.Split(',');
                    query = query.Where(r => statusArry.Contains(r.Status.ToString()));
                }


                var toalCount = await query.CountAsync();
                var data = query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToList();
                var ret = new List<EmployeeAskForLeaveListOutputDto>();
                foreach (var item in data)
                {
                    var entity = new EmployeeAskForLeaveListOutputDto()
                    {
                        Id = item.Id,
                        BeginTime = item.BeginTime,
                        CreationTime = item.CreationTime,
                        EndTime = item.EndTime,
                        DepartmentName = item.OrgName,
                        Reason = item.Reason,
                        Remark = item.Remark,
                        Status = item.Status,
                        UserId = item.UserId,
                        UserId_Name = item.UserId_Name,
                        TenantId = item.TenantId,
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
                return new PagedResultDto<EmployeeAskForLeaveListOutputDto>(toalCount, ret);
            }

        }


        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<int> GetEmployeeAskForLeaveMonthCount()
        {
            var month = DateTime.Now.Month;
            var count = _repository.GetAll().Where(x=>x.BeginTime.Month==month).GroupBy(x => x.UserId).Count();
            return count;
        }


        /// <summary>
        /// 添加一个EmployeeAskForLeave
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        [AbpAuthorize]
        public async Task<InitWorkFlowOutput> Create(CreateEmployeeAskForLeaveIputDto input)
        {
            if (input.RelationUserId.HasValue)
            {
                if (_roleRelationManager.IsExistence(input.RelationUserId.Value, input.BeginTime, input.EndTime))
                    throw new UserFriendlyException("委托人已经被委托了。");
            }
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            if (_repository.Count(r => r.CreatorUserId == AbpSession.UserId.Value && r.Status == -1) != _repository.Count(r => r.CreatorUserId == AbpSession.UserId.Value &&
            (r.BeginTime > input.EndTime || r.EndTime < input.BeginTime) && r.Status == -1))
                throw new UserFriendlyException("已存在审批通过的请假日期");

            var ret = new InitWorkFlowOutput();
            var newmodel = new EmployeeAskForLeave()
            {
                Id = Guid.NewGuid(),
                UserId = AbpSession.UserId.Value,
                BeginTime = input.BeginTime,
                EndTime = input.EndTime,
                Reason = input.Reason,
                Remark = input.Remark,
                TenantId = AbpSession.TenantId,
                OrgId = userOrgModel.OrgId,
                RelationUserId = input.RelationUserId,
                Hours = input.Hours,
                PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId)),
            };
            
            await _repository.InsertAsync(newmodel);
            ret.InStanceId = newmodel.Id.ToString();
            return ret;
        }


        [AbpAuthorize]
        public async Task Update(UpdateEmployeeAskForLeaveIputDto input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var dbmodel = await _repository.GetAsync(input.Id);
            var old_Model = dbmodel.DeepClone<EmployeeAskForLeave>();
            dbmodel.BeginTime = input.BeginTime;
            dbmodel.EndTime = input.EndTime;
            dbmodel.Reason = input.Reason;
            dbmodel.Remark = input.Remark;
            dbmodel.Hours = input.Hours;
            dbmodel.RelationUserId = input.RelationUserId;
            dbmodel.OrgId = userOrgModel.OrgId;
            dbmodel.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            await _repository.UpdateAsync(dbmodel);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(dbmodel));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }

        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task DeleteEmployeeAskForLeave(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }


        public async Task<List<ChangeLog>> GetChangeLog(EntityDto<Guid> input, Guid flowId)
        {
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
            var tableName = flowModel.TitleField.Table;
            var query = (from a in _projectAuditRepository.GetAll()
                         where a.InstanceId == input.Id.ToString() && a.TableName == tableName
                         group a by new { a.GroupId } into g
                         select new { CreatTime = g.First().CreationTime, g }).OrderByDescending(c => c.CreatTime);

            var logs = await query.ToListAsync();
            var ret = new List<ChangeLog>();
            foreach (var item in logs)
            {
                var firstModel = item.g.FirstOrDefault();
                var entity = new ChangeLog()
                {
                    ChangeTime = firstModel.CreationTime,
                    UserName = (await UserManager.GetUserByIdAsync(firstModel.CreatorUserId.Value)).Name,
                };
                //entity.Content = ModelColumnLogsHepler.MakeContent(item.g.ToList());
                foreach (var model in item.g)
                {
                    var logModel = new LogColumnModel()
                    {
                        ChangeType = model.ChangeType,
                        FieldName = model.FieldName,
                        NewValue = model.NewValue,
                        OldValue = model.OldValue,
                    };
                    entity.ContentModel.Add(logModel);
                }
                ret.Add(entity);
            }
            return ret;
        }


        private EmployeeAskForLeaveChangeDto GetChangeModel(EmployeeAskForLeave model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<EmployeeAskForLeaveChangeDto>();
            if (model.RelationUserId.HasValue)
            {
                var user = UserManager.Users.FirstOrDefault(x => x.Id == model.RelationUserId.Value);
                if (user != null)
                    ret.RelationUserId_Name = user.Name;
            }
            return ret;
        }

        public void AskForLeaveRelationUserId(Guid instanceID)
        {
            var model = _repository.Get(instanceID);
            if (model.Status == -1 && model.RelationUserId.HasValue) {
                var input = new CreateRoleRelationInput()
                {
                    UserId = model.UserId,
                    RelationUserId = model.RelationUserId.Value,
                    StartTime = model.BeginTime,
                    Type = RelationType.AskForLeave,
                    RelationId = model.Id,
                    EndTime = model.EndTime
                };
                _roleRelationManager.Create(input);
            }
        }
    }
}
