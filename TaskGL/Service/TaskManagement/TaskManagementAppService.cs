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
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.Authorization;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using TaskGL.Enum;
using ZCYX.FRMSCore.Users;

namespace TaskGL
{
    public class TaskManagementAppService : FRMSCoreAppServiceBase, ITaskManagementAppService
    {
        private readonly IRepository<TaskManagement, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly IRepository<Follow, Guid> _followRepository;
        private readonly IRepository<TaskManagementRecord, Guid> _taskManagementRecordRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;

        private readonly EmployeesSignManager _employeesSignManager;
        public TaskManagementAppService(IRepository<TaskManagement, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager,
            WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository, IRepository<Follow, Guid> followRepository
            , IRepository<TaskManagementRecord, Guid> taskManagementRecordRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, EmployeesSignManager employeesSignManager
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _followRepository = followRepository;
            _taskManagementRecordRepository = taskManagementRecordRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _employeesSignManager = employeesSignManager;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<TaskManagementListOutputDto>> GetList(GetTaskManagementListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join bd in UserManager.Users on a.UserId equals bd.Id into g
                        from b in g.DefaultIfEmpty()
                        join ff in _followRepository.GetAll().Where(x => x.BusinessType == FollowType.任务管理 && x.CreatorUserId == AbpSession.UserId.Value) on a.Id equals ff.BusinessId into tmp2
                        from h in tmp2.DefaultIfEmpty()
                        join ee in UserManager.Users on a.Superintendent equals ee.Id into m
                        from e in m.DefaultIfEmpty()
                        join k in UserManager.Users on a.TaskCreateUserId equals k.Id
                        let openModel = (from b in _workFlowTaskRepository.GetAll()
                                         .Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value)
                                         select b)
                        select new TaskManagementListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Status = a.Status ?? 0,
                            UserId = a.UserId,
                            UserName = b == null ? "" : b.Name,
                            IsFollow = h != null,
                            Type = a.Type,
                            EndTime = a.EndTime,
                            TaskName = a.TaskName,
                            IsUrgent = a.IsUrgent,
                            Superintendent = a.Superintendent,
                            SuperintendentName = e == null ? "" : e.Name,
                            TaskStatus = a.TaskStatus,
                            Number = a.Number,
                            ProjectName = a.ProjectName,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                            TaskCreateUserId = a.TaskCreateUserId,
                            TaskCreateUserName = k.Name,
                        };
            if (input.TaskStatus.HasValue)
                query = query.Where(r => r.TaskStatus == input.TaskStatus.Value);

            if (input.Type.HasValue)
                query = query.Where(r => r.Type == input.Type.Value);
            if (!input.SearchKey.IsNullOrEmpty())
                query = query.Where(r => r.TaskName.Contains(input.SearchKey) || r.TaskCreateUserName.Contains(input.SearchKey) || r.SuperintendentName.Contains(input.SearchKey)
                || r.UserName.Contains(input.SearchKey));

            if (input.IsFollow.HasValue)
                query = query.Where(r => r.IsFollow);
            if (input.GetMy)
                query = query.Where(r => r.UserId.HasValue && r.UserId.Value == AbpSession.UserId.Value);

            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                item.TypeName = item.Type.GetLocalizedDescription();
                item.TaskStatusTitle = item.TaskStatus.GetLocalizedDescription();
            }
            return new PagedResultDto<TaskManagementListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<TaskManagementOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<TaskManagementOutputDto>();
            ret.TypeName = ret.Type.GetLocalizedDescription();
            if (ret.UserId.HasValue)
                ret.UserName = (await UserManager.GetUserByIdAsync(ret.UserId.Value)).Name;
            ret.TaskCreateUserName = (await UserManager.GetUserByIdAsync(ret.TaskCreateUserId)).Name;
            if (ret.Superintendent.HasValue)
                ret.SuperintendentName = (await UserManager.GetUserByIdAsync(ret.Superintendent.Value)).Name;
            return ret;
        }
        /// <summary>
        /// 添加一个TaskManagement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateTaskManagementInput input)
        {
            var newmodel = new TaskManagement()
            {
                Requirement = input.Requirement,
                SignFileId = input.SignFileId,
                UserId = input.UserId,
                Type = input.Type,
                EndTime = input.EndTime,
                TaskName = input.TaskName,
                IsUrgent = input.IsUrgent,
                Superintendent = input.Superintendent,
                Explain = input.Explain,
                PerformanceScore = input.PerformanceScore,
                SpiritScore = input.SpiritScore,
                ProjectId = input.ProjectId,
                TaskStatus = input.TaskStatus,
                TaskManagementIId = input.TaskManagementIId,
                Number = input.Number,
                ProjectName = input.ProjectName,
                TaskCreateUserId = input.TaskCreateUserId,
                CreateUserRoleType = GetCreateRoleType(AbpSession.UserId.Value),
            };
            var signFileId = _employeesSignManager.GetSignFileId(input.TaskCreateUserId);
            if (signFileId.HasValue)
                newmodel.SignFileId = signFileId;

            if (newmodel.CreateUserRoleType != CreateUserRoleTypeEnum.BGSZR)
                newmodel.TaskCreateUserId = AbpSession.UserId.Value;
            newmodel.Status = 0;
            
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个TaskManagement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateTaskManagementInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new TaskManagement();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<TaskManagement>();
                }
                dbmodel.Requirement = input.Requirement;
                dbmodel.SignFileId = input.SignFileId;
                dbmodel.UserId = input.UserId;
                dbmodel.Type = input.Type;
                dbmodel.EndTime = input.EndTime;
                dbmodel.TaskName = input.TaskName;
                dbmodel.IsUrgent = input.IsUrgent;
                dbmodel.Superintendent = input.Superintendent;
                dbmodel.Explain = input.Explain;
                dbmodel.PerformanceScore = input.PerformanceScore;
                dbmodel.SpiritScore = input.SpiritScore;
                dbmodel.ProjectId = input.ProjectId;
                dbmodel.TaskStatus = input.TaskStatus;
                dbmodel.TaskManagementIId = input.TaskManagementIId;
                dbmodel.Number = input.Number;
                dbmodel.ProjectName = input.ProjectName;
                if (dbmodel.CreateUserRoleType == CreateUserRoleTypeEnum.BGSZR)
                    dbmodel.TaskCreateUserId = input.TaskCreateUserId;

                await _repository.UpdateAsync(dbmodel);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.InStanceId.ToString(), flowModel.TitleField.Table);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 修改一个TaskManagement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task UpdateByChangeId(UpdateTaskManagementByChangeIdInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new TaskManagement();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<TaskManagement>();
                }
                dbmodel.Requirement = input.Requirement;
                dbmodel.SignFileId = input.SignFileId;
                dbmodel.UserId = input.UserId;
                dbmodel.Type = input.Type;
                dbmodel.EndTime = input.EndTime;
                dbmodel.TaskName = input.TaskName;
                dbmodel.IsUrgent = input.IsUrgent;
                dbmodel.Superintendent = input.Superintendent;
                dbmodel.Explain = input.Explain;
                dbmodel.PerformanceScore = input.PerformanceScore;
                dbmodel.SpiritScore = input.SpiritScore;
                dbmodel.ProjectId = input.ProjectId;
                dbmodel.TaskStatus = input.TaskStatus;
                dbmodel.TaskManagementIId = input.TaskManagementIId;
                dbmodel.Number = input.Number;
                dbmodel.ProjectName = input.ProjectName;
                dbmodel.TaskCreateUserId = input.TaskCreateUserId;

                await _repository.UpdateAsync(dbmodel);
                if (input.IsUpdateForChange && input.ChangeId.HasValue)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var oldModel =await GetChangeModel(logModel);
                    var logs = oldModel.GetColumnAllLogs(await GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.ChangeId.ToString(), flowModel.TitleField.Table);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        private async Task<TaskManagementLogDto> GetChangeModel(TaskManagement model)
        {
            var ret = model.MapTo<TaskManagementLogDto>();
            ret.UserName = model.UserId == null ? "" : (await UserManager.GetUserByIdAsync(model.UserId.Value)).Name;
            ret.SuperintendentName = model.Superintendent == null ? "" : (await UserManager.GetUserByIdAsync(model.Superintendent.Value)).Name;
            ret.TypeName = model.Type.GetLocalizedDescription();
            return ret;
        }
        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        public bool IsLeader()
        {
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            if (userRoles.Any(r => string.Compare(r, "BGSZR", true) == 0))
                return false;
            else if (userRoles.Any(r => string.Compare(r, "DLEADER", true) == 0 || string.Compare(r, "OrgFGLD", true) == 0 || string.Compare(r, "ZJL", true) == 0))
                return true;
            else
                return false;
        }

        private CreateUserRoleTypeEnum GetCreateRoleType(long userId)
        {
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(userId);
            if (userRoles.Any(r => string.Compare(r, "BGSZR", true) == 0))
                return CreateUserRoleTypeEnum.BGSZR;
            else if (userRoles.Any(r => string.Compare(r, "ZJL", true) == 0))
                return CreateUserRoleTypeEnum.ZJL;
            else if (userRoles.Any(r => string.Compare(r, "OrgFGLD", true) == 0))
                return CreateUserRoleTypeEnum.OrgFGLD;
            else if (userRoles.Any(r => string.Compare(r, "DLEADER", true) == 0))
                return CreateUserRoleTypeEnum.DepartLeader;
            else
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该人员不能创建任务");
        }

        /// <summary>
        /// 更新任务工作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SaveRecore(CreateOrUpdateTaskManagementRecordInput input)
        {
            var data = await _repository.GetAsync(input.TaskManagementId);
            if (data.Type != Enum.TaskManagementTypeEnum.Offline)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "线下任务才能提交任务记录");

            if ((int)input.TaskStatus > 2)
            {
                data.TaskStatus = input.TaskStatus;
                await _repository.UpdateAsync(data);
                var users = data.UserId + ","  + data.Superintendent;
                var taskUsers = string.Join(',', _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == data.Id.ToString()).Select(x => x.ReceiveID).ToList());
                users += "," + taskUsers;
                var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
                var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
                noticeInput.Content = $"{data.TaskStatus.GetLocalizedDescription()}  通知 <a class='ivu-table' href='/#/task/detail?instanceID={data.Id}'>查看详细</a>";
                noticeInput.Title = $"{data.TaskName}  {data.TaskStatus.GetLocalizedDescription()} ";
                noticeInput.NoticeUserIds = users;
                noticeInput.NoticeType = 1;
                noticeInput.SendUserId = data.CreatorUserId.Value;
                noticeService.CreateOrUpdateNotice(noticeInput);
            }
            if (input.Id.HasValue)
            {
                var model = await _taskManagementRecordRepository.GetAsync(input.Id.Value);
                model.Content = input.Content;
                await _taskManagementRecordRepository.UpdateAsync(model);
            }
            else
            {
                var newModel = new TaskManagementRecord()
                {
                    Id = Guid.NewGuid(),
                    Content = input.Content,
                    TaskManagementId = input.TaskManagementId,
                };
                await _taskManagementRecordRepository.InsertAsync(newModel);
            }
        }


        /// <summary>
        /// 获取任务工作记录
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <returns></returns>
        public async Task<TaskManagementRecordOutput> GetRecordByTaskId(Guid id)
        {
            var data = await _repository.GetAsync(id);
            if (data.Type != Enum.TaskManagementTypeEnum.Offline)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "线下任务才能提交任务记录");

            var model = await _taskManagementRecordRepository.FirstOrDefaultAsync(r => r.TaskManagementId == id);
            if (model == null)
                return new TaskManagementRecordOutput();
            else
            {
                return new TaskManagementRecordOutput()
                {
                    Id = model.Id,
                    Content = model.Content,
                    TaskStatus = data.TaskStatus,
                    TaskManagementId = model.TaskManagementId
                };
            }
        }


        [RemoteService(IsEnabled = false)]
        public void UpdateTaskStatus(Guid id, TaskManagementStateEnum status)
        {
            var model = _repository.Get(id);
            model.TaskStatus = status;
            _repository.Update(model);

        }



        /// <summary>
        /// 自动给办公室主任创建的任务， 进行加签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<string> AutoAddWriteForTask(Guid id, Guid taskId)
        {
            var model = await _repository.GetAsync(id);
            if (model.CreateUserRoleType != CreateUserRoleTypeEnum.BGSZR)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "办公室主任创建的任务才会自动加签");

            var addUserList = new List<long>();
            addUserList.Add(model.TaskCreateUserId);
            var userRoles = await UserManager.GetRolesAsync(await UserManager.GetUserByIdAsync(model.TaskCreateUserId));
            if (userRoles.Any(r => string.Compare(r, "ZJL", true) == 0))
            {
            }
            else if (userRoles.Any(r => string.Compare(r, "OrgFGLD", true) == 0))
            {
                var users = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("ZJL");
                if (users.Count == 0)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未能找到总经理");
                addUserList.Add(users.FirstOrDefault().Id);
            }
            else if (userRoles.Any(r => string.Compare(r, "DLEADER", true) == 0))
            {

                var chargeleaders = _workFlowOrganizationUnitsManager.GetChargeLeaderUsers(model.TaskCreateUserId).FirstOrDefault();
                if (chargeleaders == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未能找到任务发起者的部门分管领导");
                addUserList.Add(chargeleaders.Id);


                var users = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("ZJL");
                if (users.Count == 0)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未能找到总经理");
                addUserList.Add(users.FirstOrDefault().Id);

            }
            else
            {

            }
            var addUsers = string.Join(",", addUserList.Distinct().Select(r => $"{MemberPerfix.UserPREFIX}{r}"));
            var workFlowWorkTaskAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            var output = workFlowWorkTaskAppService.AddWrite(taskId, 2, 3, addUsers, "");
            return output.ReturnMsg;

        }


        public void CreateComplete(Guid id)
        {
            var model = _repository.Get(id);
            if (model.CreateUserRoleType == CreateUserRoleTypeEnum.BGSZR)
                model.TaskStatus = TaskManagementStateEnum.BefreStart;
            else
                model.TaskStatus = TaskManagementStateEnum.Approvaling;
        }



        /// <summary>
        /// 查找审批人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public string FindAuditUser(Guid id, int actionType)
        {
            var model = _repository.Get(id);
            if (model.CreateUserRoleType == CreateUserRoleTypeEnum.BGSZR)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "办公室主任创建的任务不需要进行审批");
            if (actionType == 1)
            {
                return $"{MemberPerfix.UserPREFIX}{model.TaskCreateUserId}";
            }
            else if (actionType == 2)
            {
                if (model.CreateUserRoleType == CreateUserRoleTypeEnum.DepartLeader)
                {
                    var chargeleaders = _workFlowOrganizationUnitsManager.GetChargeLeaderUsers(model.TaskCreateUserId).FirstOrDefault();
                    if (chargeleaders == null)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未能找到任务发起者的部门分管领导");
                    return $"{MemberPerfix.UserPREFIX}{chargeleaders.Id}";
                }
                else if (model.CreateUserRoleType == CreateUserRoleTypeEnum.OrgFGLD)
                {
                    return $"{MemberPerfix.UserPREFIX}{model.TaskCreateUserId}";
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            else if (actionType == 3)
            {
                if (model.CreateUserRoleType == CreateUserRoleTypeEnum.ZJL)
                    return $"{MemberPerfix.UserPREFIX}{model.TaskCreateUserId}";
                else
                {
                    var users = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("ZJL");
                    if (users.Count == 0)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未能找到总经理");
                    return $"{MemberPerfix.UserPREFIX}{users.FirstOrDefault().Id}";
                }
            }
            else
                throw new NotImplementedException();

        }

    }
}