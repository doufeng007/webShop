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
using Abp.UI;
using Newtonsoft.Json.Linq;
using Abp.Extensions;
using Abp.Runtime.Caching;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using Abp.Domain.Uow;
using Abp.Authorization;
using Abp.WorkFlowDictionary;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Application;
using Abp.SignalR.Core;
using Dapper;
using Abp.File;
using ZCYX.FRMSCore.Model;
using Abp.Events.Bus;

namespace Abp.WorkFlow
{
    public class WorkFlowWorkTaskAppService : ApplicationService, IWorkFlowWorkTaskAppService
    {
        private readonly IRepository<User, long> _useRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<WorkFlow, Guid> _workFlowRepository;
        private readonly EmployeesSignManager _employeesSignManager;
        private readonly ISqlQeuryRepository _coreSqlQeuryRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryrepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<RoleRelation, Guid> _roleRelationRepository;
        private readonly IRepository<TaskManagementRelation, Guid> _taskManagementRelation;
        public IEventBus _eventBus { get; set; }
        public WorkFlowWorkTaskAppService(IRepository<User, long> useRepository, IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IWorkFlowTaskRepository workFlowTaskRepository
            , IRepository<WorkFlow, Guid> workFlowRepository, IRepository<AbpDictionary, Guid> abpDictionaryrepository, WorkFlowTaskManager workFlowTaskManager
            , ISqlQeuryRepository coreSqlQeuryRepository, IDynamicRepository dynamicRepository, WorkFlowCacheManager workFlowCacheManager, IAbpFileRelationAppService abpFileRelationAppService, EmployeesSignManager employeesSignManager, IRepository<RoleRelation, Guid> roleRelationRepository, IRepository<TaskManagementRelation, Guid> taskManagementRelation)
        {
            this._useRepository = useRepository;
            _organizeRepository = organizeRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowRepository = workFlowRepository;
            _coreSqlQeuryRepository = coreSqlQeuryRepository;
            _abpDictionaryrepository = abpDictionaryrepository;
            _workFlowTaskManager = workFlowTaskManager;
            _dynamicRepository = dynamicRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _roleRelationRepository = roleRelationRepository;
            _employeesSignManager = employeesSignManager;
            _eventBus = NullEventBus.Instance;
            _taskManagementRelation = taskManagementRelation;
        }
        public async Task GetRelation()
        {
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var dt = DateTime.Now;
            var list = _roleRelationRepository.GetAll().Where(x => x.EndTime < dt).ToList();
            foreach (var newmodel in list)
            {
                organizeManager.DeleteRelationRoleAndPermission(newmodel.Id);
                _roleRelationRepository.Delete(newmodel);
                var tasks = _workFlowTaskRepository.GetAll().Where(x => x.RelationId == newmodel.Id && x.Status == 0).ToList();
                foreach (var item in tasks)
                {
                    item.ReceiveID = newmodel.UserId;
                    var user = _useRepository.Get(newmodel.UserId);
                    item.ReceiveName = user.Name;
                    _workFlowTaskRepository.Update(item);
                }
            }
        }

        public async Task UpdateTaskFiles(TaskFileInput input)
        {
            var currentTask = await _workFlowTaskRepository.GetAsync(input.TaskId);
            currentTask.Comment = input.Comment;
            await _workFlowTaskRepository.UpdateAsync(currentTask);
            var fileList = new List<AbpFileListInput>();
            if (input.FlowFileList != null)
            {
                foreach (var item in input.FlowFileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
            }
            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = input.TaskId.ToString(),
                BusinessType = (int)AbpFileBusinessType.流程意见,
                Files = fileList
            });
        }

        [AbpAuthorize]
        public async Task<InitWorkFlowOutput> InitWorkFlowInstanceAsync(InitWorkFlowInput input)
        {
            var bworkFlow = new WorkFlow();
            var currentUser = await _useRepository.GetAsync(AbpSession.UserId.Value);
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var instalwf = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            var firstStepModel = instalwf.Steps.FirstOrDefault();
            var model = new WorkFlowTask() { };
            model.Id = Guid.NewGuid();
            model.PrevID = Guid.Empty;
            model.PrevStepID = Guid.Empty;
            model.FlowID = input.FlowId;
            model.StepID = firstStepModel.ID;
            model.StepName = firstStepModel.Name;
            model.InstanceID = input.InStanceId;
            model.GroupID = Guid.NewGuid();
            model.Type = 0;
            model.Title = input.FlowTitle;
            model.SenderID = currentUser.Id;
            model.SenderName = currentUser.Name;
            model.SenderTime = DateTime.Now;
            model.ReceiveID = currentUser.Id;
            model.ReceiveName = currentUser.Name;
            model.ReceiveTime = DateTime.Now;
            model.Status = 0;
            model.Sort = 1;
            model.Deepth = 1;
            model.OtherType = 0;
            model.VersionNum = instalwf.VersionNum;
            var taskId = await _workFlowTaskRepository.InsertAndGetIdAsync(model);
            var ret = new InitWorkFlowOutput();
            ret.FlowId = input.FlowId;
            ret.InStanceId = input.InStanceId;
            ret.TaskId = taskId;
            ret.GroupId = model.GroupID;
            ret.StepId = model.StepID;
            return ret;
        }

        [AbpAuthorize]
        public InitWorkFlowOutput InitWorkFlowInstance(InitWorkFlowInput input)
        {
            var bworkFlow = new WorkFlow();
            var currentUser = _useRepository.Get(AbpSession.UserId.Value);
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var instalwf = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            var firstStepModel = instalwf.Steps.FirstOrDefault();
            var model = new WorkFlowTask() { };
            model.Id = Guid.NewGuid();
            model.PrevID = Guid.Empty;
            model.PrevStepID = Guid.Empty;
            model.FlowID = input.FlowId;
            model.StepID = firstStepModel.ID;
            model.StepName = firstStepModel.Name;
            model.InstanceID = input.InStanceId;
            model.GroupID = Guid.NewGuid();
            model.Type = 0;
            model.Title = input.FlowTitle;
            model.SenderID = currentUser.Id;
            model.SenderName = currentUser.Name;
            model.SenderTime = DateTime.Now;
            if (input.ReciveUserId.HasValue)
            {
                var recevieUser = _useRepository.Get(input.ReciveUserId.Value);
                model.ReceiveID = recevieUser.Id;
                model.ReceiveName = recevieUser.Name;
            }
            else
            {
                model.ReceiveID = currentUser.Id;
                model.ReceiveName = currentUser.Name;
            }
            model.ReceiveTime = DateTime.Now;
            model.Status = firstStepModel.IsHideTask ? -1 : 0;
            model.Sort = 1;
            model.OtherType = 0;
            model.Deepth = 1;
            model.VersionNum = instalwf.VersionNum;
            var taskId = _workFlowTaskRepository.InsertAndGetId(model);
            var ret = new InitWorkFlowOutput();
            ret.FlowId = input.FlowId;
            ret.InStanceId = input.InStanceId;
            ret.TaskId = taskId;
            ret.GroupId = model.GroupID;
            ret.StepId = model.StepID;
            ret.StepName = model.StepName;
            //关联任务编号
            if (input.RelationTaskId.HasValue)
            {
                _taskManagementRelation.Insert(new TaskManagementRelation()
                {
                    Id = Guid.NewGuid(),
                    FlowId = input.FlowId,
                    InStanceId = input.InStanceId.ToGuid(),
                    TaskManagementId = input.RelationTaskId.Value,
                });
                _eventBus.Trigger(new TaskManagementData() { Id = input.RelationTaskId.Value, TaskStatus = TaskManagementStateEnum.Doing });
            }
            return ret;
        }


        public void InitWorkFlowInstanceByRole(InitWorkFlowInput input, long userId, int roleId)
        {
            var bworkFlow = new WorkFlow();
            var currentUser = _useRepository.Get(userId);
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var instalwf = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            var firstStepModel = instalwf.Steps.FirstOrDefault();
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var defaultMember = organizeManager.GetAbpUserIdByRoleId(roleId).ToString().TrimEnd(',');
            var members = defaultMember.Split(",");
            foreach (var item in members)
            {
                var id = item.Replace("u_", "");
                var receiveUser = _useRepository.Get(Convert.ToInt64(id));
                InsertWorkFlowTask(input, firstStepModel, instalwf.VersionNum, currentUser, receiveUser);
            }
        }


        public void InsertWorkFlowTask(InitWorkFlowInput input, WorkFlowStep stepModel, int versionNum, User currentUser, User receiveUser)
        {
            var model = new WorkFlowTask() { };
            model.Id = Guid.NewGuid();
            model.PrevID = Guid.Empty;
            model.PrevStepID = Guid.Empty;
            model.FlowID = input.FlowId;
            model.StepID = stepModel.ID;
            model.StepName = stepModel.Name;
            model.InstanceID = input.InStanceId;
            model.GroupID = Guid.NewGuid();
            model.Type = 0;
            model.Title = input.FlowTitle;
            model.SenderID = currentUser.Id;
            model.SenderName = currentUser.Name;
            model.SenderTime = DateTime.Now;
            model.ReceiveID = receiveUser.Id;
            model.ReceiveName = receiveUser.Name;
            model.ReceiveTime = DateTime.Now;
            model.Status = 0;
            model.Sort = 1;
            model.OtherType = 0;
            model.Deepth = 1;
            model.VersionNum = versionNum;
            _workFlowTaskRepository.InsertAndGetId(model);
        }


        public Guid InsertWorkFlowTaskForUserId(Guid flowId, string instanceId, string flowTitle, long reciveUserId)
        {
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var instalwf = workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            var model = new WorkFlowTask() { };
            model.Id = Guid.NewGuid();
            model.PrevID = Guid.Empty;
            model.PrevStepID = Guid.Empty;
            model.FlowID = flowId;
            var firstStepModel = instalwf.Steps.FirstOrDefault();
            model.StepID = firstStepModel.ID;
            model.StepName = firstStepModel.Name;
            model.InstanceID = instanceId;
            model.GroupID = Guid.NewGuid();
            model.Type = 0;
            model.Title = flowTitle;
            model.SenderID = 1;
            model.SenderName = "管理员";
            model.SenderTime = DateTime.Now;
            model.ReceiveID = reciveUserId;
            var recevieUser = _useRepository.Get(reciveUserId);
            model.ReceiveName = recevieUser.Name;
            model.ReceiveTime = DateTime.Now;
            model.Status = 0;
            model.Sort = 1;
            model.OtherType = 0;
            model.Deepth = 1;
            model.VersionNum = instalwf.VersionNum;
            return _workFlowTaskRepository.InsertAndGetId(model);

        }



        public async Task<ExecuteWorkFlowOutput> VailitionTaskStatus(Guid taskId)
        {
            var ret = new ExecuteWorkFlowOutput();
            var taskModel = await _workFlowTaskRepository.GetAsync(taskId);
            if (taskModel.Status > 2)
            {
                ret.IsSuccesefull = false;
                ret.ErrorMsg = "任务已处理";
                ret.ReturnMsg = "任务已处理";
            }
            else
                ret.IsSuccesefull = true;
            return ret;
        }
        public async Task UpdateOpenTaskStatus(Guid taskId)
        {
            var taskModel = await _workFlowTaskRepository.GetAsync(taskId);
            if (taskModel.Status == -1)
            {
                taskModel.Status = 0;
                await _workFlowTaskRepository.UpdateAsync(taskModel);
            }
        }



        [UnitOfWork]
        [AbpAuthorize]
        public async Task<ExecuteWorkFlowOutput> ExecuteTask(ExecuteWorkFlowInput input)
        {
            var valResult = ValidationExecute(input);
            if (!valResult.IsSuccesefull)
            {
                valResult.ErrorMsg = "流程未完成，暂不能执行";
                return valResult;
            }
            var resultOutput = new ExecuteWorkFlowOutput();
            if (input.ActionType.IsNullOrWhiteSpace())
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "参数为空";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            string opation = input.ActionType.ToLower();
            var currentTask = await _workFlowTaskRepository.GetAsync(input.TaskId);
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, currentTask.VersionNum);
            //var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            if (workflowRunModel == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到流程";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var execute = new WorkFlowExecute();
            execute.Comment = input.Comment;
            switch (opation)
            {
                case "freesubmit":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Submit;
                    execute.OtherType = 1;//自由流程发送
                    break;
                case "submit":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Submit;
                    break;
                case "save":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Save;
                    break;
                case "back":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Back;
                    break;
                case "completed":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Completed;
                    break;
                case "redirect":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Redirect;
                    break;
                case "addwrite":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.AddWrite;
                    break;
                case "copyforcompleted":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.CopyforCompleted;
                    break;
                case "inquirycompleted":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.InquiryCompleted;
                    break;
            }
            execute.FlowID = input.FlowId;
            execute.GroupID = input.GroupId;
            execute.InstanceID = input.InstanceId;
            execute.IsSign = input.IsSign;
            execute.Note = "";
            execute.Sender = await _useRepository.GetAsync(AbpSession.UserId.Value);
            execute.StepID = input.StepId;
            execute.TaskID = input.TaskId;
            execute.VersionNum = currentTask.VersionNum;
            execute.Title = input.Title ?? "";
            var currentSteps = workflowRunModel.Steps.Where(p => p.ID == execute.StepID);
            if (!currentSteps.Any())
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到当前步骤";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var currrentSetp = currentSteps.First();
            var stepDatas = input.Steps;
            execute.Users = input.Users;
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();

            //if (stepsjson.IsArray)
            if (stepDatas.Count > 0)
            {
                foreach (var step in stepDatas)
                {
                    string id = step.id;
                    string member = step.member;
                    Guid gid;
                    if (id.IsGuid(out gid))
                    {
                        switch (execute.ExecuteType)
                        {
                            case WorkFlowEnumType.WorkFlowExecuteType.Submit:
                                execute.Steps.Add(gid, await organizeManager.GetAllUsersAsync(member));
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Back:
                                execute.Steps.Add(gid, currrentSetp.Behavior.BackModel == 4 ? await organizeManager.GetAllUsersAsync(member) : new List<User>());
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Save:
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Completed:
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Redirect:
                                break;
                        }
                    }

                    if (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Redirect)
                    {
                        execute.Steps.Add(Guid.Empty, await organizeManager.GetAllUsersAsync(member));
                    }
                }
            }

            var eventParams = new WorkFlowCustomEventParams();
            eventParams.FlowID = execute.FlowID;
            eventParams.GroupID = execute.GroupID;
            eventParams.StepID = execute.StepID;
            eventParams.TaskID = execute.TaskID;
            eventParams.InstanceID = execute.InstanceID;
            eventParams.OtherString = input.Parameters;

            //保存业务数据 "1" != Request.QueryString["isSystemDetermine"]:当前步骤流转类型如果是系统判断，则先保存数据，在这里就不需要保存数据了。 暂不做流程里面保存业务数据
            var steps = workflowRunModel.Steps.Where(p => p.ID == execute.StepID);
            if (currentTask.Type != 5)
            {
                foreach (var step in steps)
                {
                    //步骤提交前事件
                    if (!step.Event.SubmitBefore.IsNullOrEmpty() &&
                        (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit
                        || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed))
                    {
                        ///提交步骤前事件 增加业务逻辑验证， 是否能提交； 统一返回bool
                        object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.SubmitBefore.Trim(), eventParams);
                        var valiationRet = obj as ExecuteWorkFlowOutput;
                        if (valiationRet != null && !valiationRet.IsSuccesefull)
                            return valiationRet;
                    }
                    //步骤退回前事件
                    if (!step.Event.BackBefore.IsNullOrEmpty() && execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back)
                    {
                        object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.BackBefore.Trim(), eventParams);
                    }
                }
            }

            //处理委托 暂无委托
            //foreach (var executeStep in execute.Steps)
            //{
            //    for (int i = 0; i < executeStep.Value.Count; i++)
            //    {
            //        Guid newUserID = bworkFlowDelegation.GetFlowDelegationByUserID(execute.FlowID, executeStep.Value[i].ID);
            //        if (newUserID != Guid.Empty && newUserID != executeStep.Value[i].ID)
            //        {
            //            executeStep.Value[i] = busers.Get(newUserID);
            //        }
            //    }
            //}
            var reslut = new WorkFlowResult();
            try
            {
                reslut = workFlowTaskManager.Execute(execute);
                if (!workflowRunModel.IsFiles)
                {
                    var fileList = new List<AbpFileListInput>();
                    if (input.FileList != null)
                    {
                        foreach (var item in input.FileList)
                        {
                            fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                        }
                    }
                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = input.TaskId.ToString(),
                        BusinessType = (int)AbpFileBusinessType.流程意见,
                        Files = fileList
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            if (reslut.IsSuccess)
            {

                //SignalR暂时屏蔽 后面需要处理
                var signalrNoticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISignalrNoticeAppService>();
                foreach (var m in reslut.Send)
                {
                    signalrNoticeService.SendNoticeToClientV2(m.RecieveId, input.InstanceId, m.Title, m.Content, m.LinkUrl);
                }
            }


            string msg = string.Empty;
            if (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back && !currrentSetp.BackShowMsg.IsNullOrEmpty()
                && execute.ExecuteType != WorkFlowEnumType.WorkFlowExecuteType.AddWrite)
            {
                msg = currrentSetp.BackShowMsg;
            }
            else if (((execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed
                || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit) && !currrentSetp.SendShowMsg.IsNullOrEmpty())
                && execute.ExecuteType != WorkFlowEnumType.WorkFlowExecuteType.AddWrite)
            {
                msg = currrentSetp.SendShowMsg;
            }
            else
            {
                msg = reslut.Messages;
            }
            string logContent = string.Format("处理参数：{0}<br/>处理结果：{1}<br/>调试信息：{2}<br/>返回信息：{3}",
                input.Parameters,
                reslut.IsSuccess ? "成功" : "失败",
                reslut.DebugMessages,
                reslut.Messages
                );

            Abp.Logging.LogHelper.Logger.Info($"处理了流程({workflowRunModel.Name})");


            if (reslut.IsSuccess)
            {
                if (currentTask.Type != 5)
                {
                    #region  步骤发送 退回后状态的迁移
                    if (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit
                      || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back)
                    {
                        foreach (var step in input.Steps)
                        {
                            var stepgid = step.id.ToGuid();
                            var toStepModel = workflowRunModel.Steps.FirstOrDefault(r => r.ID == stepgid);
                            if (toStepModel == null)
                                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数异常");
                            if (toStepModel.ChangeStatus)
                            {
                                await UpdateInstanceStatusAsync(workflowRunModel, toStepModel.StepToStatus, execute.TaskID, execute.FlowID, execute.InstanceID);
                            }



                            ///无论是否是子流程 都改变主流程的标识字段
                            //if (toStepModel.Type == "subflow")
                            //{
                            //    if (toStepModel.SubFlowID.IsNullOrWhiteSpace() || !toStepModel.SubFlowID.IsGuid())
                            //        throw new UserFriendlyException("子流程步骤子流程编号异常");
                            //    var subflowModel = workFlowCacheManager.GetWorkFlowModelFromCache(toStepModel.SubFlowID.ToGuid());
                            //    await UpdateInstanceStatus(subflowModel, toStepModel.StepToStatus, execute.TaskID, execute.FlowID, execute.InstanceID);
                            //}
                            //else
                            //{

                            //}
                        }
                    }

                    #endregion


                    foreach (var step in steps)
                    {

                        var aftereventParams = new WorkFlowCustomEventParamsForAfterSubmit()
                        {
                            FlowID = eventParams.FlowID,
                            GroupID = eventParams.GroupID,
                            InstanceID = eventParams.InstanceID,
                            NextRecevieUserId = eventParams.NextRecevieUserId,
                            OtherString = eventParams.OtherString,
                            StepID = eventParams.StepID,
                            TaskID = eventParams.TaskID,
                            NextTasks = reslut.NextTasks.ToList(),

                        };

                        //步骤提交后事件
                        if (!step.Event.SubmitAfter.IsNullOrEmpty() &&
                            (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit
                            || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed))
                        {
                            object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.SubmitAfter.Trim(), aftereventParams);
                            // Response.Write(string.Format("执行步骤提交后事件：({0}) 返回值：{1}<br/>", step.Event.SubmitAfter.Trim(), obj.ToString()));
                        }
                        if ((execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit) && step.Behavior.IsCopyForSend)
                        {
                            _workFlowTaskManager.WrokFlowTaskCopyFor(currentTask, step, execute, step.ID, 0, 2);
                        }
                        //步骤退回后事件
                        if (!step.Event.BackAfter.IsNullOrEmpty() && execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back)
                        {
                            object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.BackAfter.Trim(), aftereventParams);
                            //  Response.Write(string.Format("执行步骤退回后事件：({0}) 返回值：{1}<br/>", step.Event.BackAfter.Trim(), obj.ToString()));
                        }
                    }
                }
            }

            //归档  暂时不实现
            if (reslut.IsSuccess)
            {
                //任务执行完后，把自动结束步骤往下走




                //判断是打开任务还是关闭窗口
                var nextTasks =
                    reslut.NextTasks.Where(
                        p => p.Status.In(0, 1) && p.ReceiveID == AbpSession.UserId.Value);
                var nextTask = nextTasks.Any() ? nextTasks.First() : null;
                if (nextTask != null)
                {
                    //hq:判断子流程进入连接
                    if (nextTask.Type == 6)
                    {
                        await CurrentUnitOfWork.SaveChangesAsync();
                        var childTask = _workFlowTaskRepository.GetAll().Where(r => r.GroupID == nextTask.SubFlowGroupID.ToGuid() && r.Status.In(0, 1) && r.ReceiveID == AbpSession.UserId.Value);
                        if (childTask.Any())
                        {
                            nextTask = childTask.First();
                            resultOutput.NextStepUrl = string.Format("/dynamicpage?fid={0}&stepID={1}&taskID={2}&groupID={3}&instanceID={4}", nextTask.FlowID, nextTask.StepID, nextTask.Id, nextTask.GroupID, nextTask.InstanceID);
                            // await UpdateOpenTime(nextTask.Id,nextTask.FlowID,nextTask.StepID);
                        }
                        else
                            resultOutput.NextStepUrl = "";
                    }
                    else
                    {
                        //await CurrentUnitOfWork.SaveChangesAsync();
                        //await UpdateOpenTime(nextTask.Id, nextTask.FlowID, nextTask.StepID);
                        resultOutput.NextStepUrl = string.Format("/dynamicpage?fid={0}&stepID={1}&taskID={2}&groupID={3}&instanceID={4}", nextTask.FlowID, nextTask.StepID, nextTask.Id, nextTask.GroupID, nextTask.InstanceID);
                    }
                }
                else
                {
                    resultOutput.NextStepUrl = "";
                }
                ///对于子流程的完成， 自动推进主流程步骤的提示信息；
                if (opation == "completed" && reslut.NextTasks != null && reslut.NextTasks.Count() > 0)
                {
                    var nsteps = new List<GetNextStepOutput>();
                    foreach (var task in reslut.NextTasks)
                    {
                        var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID, task.VersionNum);
                        var step = workflowModel.Steps.FirstOrDefault(r => r.ID == task.StepID);
                        var entityStep = new GetNextStepOutput();
                        var selectType = "";
                        var selectRang = "";
                        var defaultMember = _workFlowTaskManager.GetDefultMember(task.FlowID, step.ID, task.GroupID, step.ID, task.InstanceID, out selectType, out selectRang,
                            input.TaskId, task.VersionNum);
                        string[] array = defaultMember.Split(',');
                        var list = array.Where(x => x.StartsWith(MemberPerfix.UserPREFIX)).Select(x => Convert.ToInt64(x.Replace(MemberPerfix.UserPREFIX, ""))).ToList();
                        foreach (var item in list)
                        {
                            var dt = DateTime.Now;
                            var model = _roleRelationRepository.GetAll().FirstOrDefault(x => x.UserId == item && x.StartTime < dt && x.EndTime > dt);
                            if (model != null && !list.Contains(model.RelationUserId))
                            {
                                defaultMember = defaultMember.Replace(MemberPerfix.UserPREFIX + model.UserId, MemberPerfix.UserPREFIX + model.RelationUserId);
                            }
                        }
                        entityStep.DefaultUserId = defaultMember;
                        entityStep.DefaultUserName = organizeManager.GetNames(defaultMember);
                        entityStep.NextStepId = step.ID;
                        entityStep.NextStepName = step.Name;
                        entityStep.IsAllowChoose = step.Behavior.RunSelect != 0;
                        entityStep.SelectRangeRootId = step.Behavior.SelectRange?.Trim() ?? "";
                        nsteps.Add(entityStep);
                    }
                    resultOutput.Steps = nsteps;
                }
            }
            resultOutput.IsSuccesefull = reslut.IsSuccess;
            resultOutput.ReturnMsg = msg;
            if (!resultOutput.IsSuccesefull && resultOutput.ErrorMsg.IsNullOrWhiteSpace() && !resultOutput.ReturnMsg.IsNullOrWhiteSpace())
                resultOutput.ErrorMsg = resultOutput.ReturnMsg;

            if (reslut.NextTasks != null)
                resultOutput.Steps = reslut.NextTasks.Where(r => r.Status == 0).GroupBy(r => r.StepID).Select(r => new GetNextStepOutput()
                {
                    NextStepId = r.Key,
                    NextStepName = r.FirstOrDefault().StepName,
                    NextStepReciveUserNames = string.Join(",", r.Select(o => o.ReceiveName))
                }).ToList();
            #region  添加对自动完成步骤的自动complete

            if (input.Steps.Count == 1)
            {
                var nextStepId = input.Steps.FirstOrDefault().id.ToGuid();
                var nextStepModel = workflowRunModel.Steps.SingleOrDefault(r => r.ID == nextStepId);
                if (nextStepModel.IsAutoCompleteStep && reslut.NextTasks != null && reslut.NextTasks.Count() == 1)
                {
                    _eventBus.Trigger(new WorkFlowAutoCompleteEventData() { TaskId = reslut.NextTasks.FirstOrDefault().Id });
                    resultOutput.Steps = new List<GetNextStepOutput>();
                }
            }


            #endregion


            return resultOutput;
        }

        [UnitOfWork]
        [AbpAuthorize]
        public ExecuteWorkFlowOutput ExecuteTaskSync(ExecuteWorkFlowInput input)
        {
            return ExecuteTaskSync(input, null);
        }

        [RemoteService(IsEnabled = false)]
        public ExecuteWorkFlowOutput ExecuteTaskWithUser(ExecuteWorkFlowInput input, User doUser)
        {
            return ExecuteTaskSync(input, doUser);
        }

        [UnitOfWork]
        private ExecuteWorkFlowOutput ExecuteTaskSync(ExecuteWorkFlowInput input, User doUser = null)
        {
            var valResult = ValidationExecute(input);
            if (!valResult.IsSuccesefull)
            {
                valResult.ErrorMsg = "流程未完成，暂不能执行";
                return valResult;
            }
            var resultOutput = new ExecuteWorkFlowOutput();
            if (!AbpSession.UserId.HasValue && doUser == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "处理人为空";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.参数为空;
                return resultOutput;
            }
            var doUserId = doUser == null ? AbpSession.UserId.Value : doUser.Id;
            if (input.ActionType.IsNullOrWhiteSpace())
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "参数为空";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            string opation = input.ActionType.ToLower();
            var currentTask = _workFlowTaskRepository.Get(input.TaskId);
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, currentTask.VersionNum);
            //var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            if (workflowRunModel == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到流程";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var execute = new WorkFlowExecute();
            execute.Comment = input.Comment;
            switch (opation)
            {
                case "freesubmit":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Submit;
                    execute.OtherType = 1;//自由流程发送
                    break;
                case "submit":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Submit;
                    break;
                case "save":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Save;
                    break;
                case "back":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Back;
                    break;
                case "completed":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Completed;
                    break;
                case "redirect":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Redirect;
                    break;
                case "addwrite":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.AddWrite;
                    break;
                case "copyforcompleted":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.CopyforCompleted;
                    break;
                case "inquirycompleted":
                    execute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.InquiryCompleted;
                    break;
            }
            execute.FlowID = input.FlowId;
            execute.GroupID = input.GroupId;
            execute.InstanceID = input.InstanceId;
            execute.IsSign = input.IsSign;
            execute.Note = "";
            if (doUser == null)
                execute.Sender = _useRepository.Get(AbpSession.UserId.Value);
            else
                execute.Sender = doUser;
            execute.StepID = input.StepId;
            execute.TaskID = input.TaskId;
            execute.VersionNum = currentTask.VersionNum;
            execute.Title = input.Title ?? "";
            var currentSteps = workflowRunModel.Steps.Where(p => p.ID == execute.StepID);
            if (!currentSteps.Any())
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到当前步骤";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var currrentSetp = currentSteps.First();
            var stepDatas = input.Steps;
            execute.Users = input.Users;
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();

            //if (stepsjson.IsArray)
            if (stepDatas.Count > 0)
            {
                foreach (var step in stepDatas)
                {
                    string id = step.id;
                    string member = step.member;
                    Guid gid;
                    if (id.IsGuid(out gid))
                    {
                        switch (execute.ExecuteType)
                        {
                            case WorkFlowEnumType.WorkFlowExecuteType.Submit:
                                execute.Steps.Add(gid, organizeManager.GetAllUsers(member));
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Back:
                                execute.Steps.Add(gid, currrentSetp.Behavior.BackModel == 4 ? organizeManager.GetAllUsers(member) : new List<User>());
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Save:
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Completed:
                                break;
                            case WorkFlowEnumType.WorkFlowExecuteType.Redirect:
                                break;
                        }
                    }

                    if (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Redirect)
                    {
                        execute.Steps.Add(Guid.Empty, organizeManager.GetAllUsers(member));
                    }
                }
            }

            var eventParams = new WorkFlowCustomEventParams();
            eventParams.FlowID = execute.FlowID;
            eventParams.GroupID = execute.GroupID;
            eventParams.StepID = execute.StepID;
            eventParams.TaskID = execute.TaskID;
            eventParams.InstanceID = execute.InstanceID;
            eventParams.OtherString = input.Parameters;

            //保存业务数据 "1" != Request.QueryString["isSystemDetermine"]:当前步骤流转类型如果是系统判断，则先保存数据，在这里就不需要保存数据了。 暂不做流程里面保存业务数据
            var steps = workflowRunModel.Steps.Where(p => p.ID == execute.StepID);
            if (currentTask.Type != 5)
            {
                foreach (var step in steps)
                {
                    //步骤提交前事件
                    if (!step.Event.SubmitBefore.IsNullOrEmpty() &&
                        (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit
                        || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed))
                    {
                        ///提交步骤前事件 增加业务逻辑验证， 是否能提交； 统一返回bool
                        object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.SubmitBefore.Trim(), eventParams);
                        var valiationRet = obj as ExecuteWorkFlowOutput;
                        if (valiationRet != null && !valiationRet.IsSuccesefull)
                            return valiationRet;
                    }
                    //步骤退回前事件
                    if (!step.Event.BackBefore.IsNullOrEmpty() && execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back)
                    {
                        object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.BackBefore.Trim(), eventParams);
                    }
                }
            }

            //处理委托 暂无委托
            //foreach (var executeStep in execute.Steps)
            //{
            //    for (int i = 0; i < executeStep.Value.Count; i++)
            //    {
            //        Guid newUserID = bworkFlowDelegation.GetFlowDelegationByUserID(execute.FlowID, executeStep.Value[i].ID);
            //        if (newUserID != Guid.Empty && newUserID != executeStep.Value[i].ID)
            //        {
            //            executeStep.Value[i] = busers.Get(newUserID);
            //        }
            //    }
            //}
            var reslut = new WorkFlowResult();
            try
            {
                reslut = workFlowTaskManager.Execute(execute, false, doUser);
                if (!workflowRunModel.IsFiles)
                {
                    var fileList = new List<AbpFileListInput>();
                    if (input.FileList != null)
                    {
                        foreach (var item in input.FileList)
                        {
                            fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                        }
                    }
                    _abpFileRelationAppService.Update(new CreateFileRelationsInput()
                    {
                        BusinessId = input.TaskId.ToString(),
                        BusinessType = (int)AbpFileBusinessType.流程意见,
                        Files = fileList
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            if (reslut.IsSuccess)
            {

                //SignalR暂时屏蔽 后面需要处理
                var signalrNoticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISignalrNoticeAppService>();
                foreach (var m in reslut.Send)
                {
                    signalrNoticeService.SendNoticeToClientV2(m.RecieveId, input.InstanceId, m.Title, m.Content, m.LinkUrl);
                }
            }


            string msg = string.Empty;
            if (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back && !currrentSetp.BackShowMsg.IsNullOrEmpty()
                && execute.ExecuteType != WorkFlowEnumType.WorkFlowExecuteType.AddWrite)
            {
                msg = currrentSetp.BackShowMsg;
            }
            else if (((execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed
                || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit) && !currrentSetp.SendShowMsg.IsNullOrEmpty())
                && execute.ExecuteType != WorkFlowEnumType.WorkFlowExecuteType.AddWrite)
            {
                msg = currrentSetp.SendShowMsg;
            }
            else
            {
                msg = reslut.Messages;
            }
            string logContent = string.Format("处理参数：{0}<br/>处理结果：{1}<br/>调试信息：{2}<br/>返回信息：{3}",
                input.Parameters,
                reslut.IsSuccess ? "成功" : "失败",
                reslut.DebugMessages,
                reslut.Messages
                );

            Abp.Logging.LogHelper.Logger.Info($"处理了流程({workflowRunModel.Name})");


            if (reslut.IsSuccess)
            {
                if (currentTask.Type != 5)
                {
                    #region  步骤发送 退回后状态的迁移
                    if (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit
                      || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back)
                    {
                        foreach (var step in input.Steps)
                        {
                            var stepgid = step.id.ToGuid();
                            var toStepModel = workflowRunModel.Steps.FirstOrDefault(r => r.ID == stepgid);
                            if (toStepModel == null)
                                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数异常");
                            if (toStepModel.ChangeStatus)
                            {
                                UpdateInstanceStatus(workflowRunModel, toStepModel.StepToStatus, execute.TaskID, execute.FlowID, execute.InstanceID);
                            }



                            ///无论是否是子流程 都改变主流程的标识字段
                            //if (toStepModel.Type == "subflow")
                            //{
                            //    if (toStepModel.SubFlowID.IsNullOrWhiteSpace() || !toStepModel.SubFlowID.IsGuid())
                            //        throw new UserFriendlyException("子流程步骤子流程编号异常");
                            //    var subflowModel = workFlowCacheManager.GetWorkFlowModelFromCache(toStepModel.SubFlowID.ToGuid());
                            //    await UpdateInstanceStatus(subflowModel, toStepModel.StepToStatus, execute.TaskID, execute.FlowID, execute.InstanceID);
                            //}
                            //else
                            //{

                            //}
                        }
                    }

                    #endregion


                    foreach (var step in steps)
                    {

                        var aftereventParams = new WorkFlowCustomEventParamsForAfterSubmit()
                        {
                            FlowID = eventParams.FlowID,
                            GroupID = eventParams.GroupID,
                            InstanceID = eventParams.InstanceID,
                            NextRecevieUserId = eventParams.NextRecevieUserId,
                            OtherString = eventParams.OtherString,
                            StepID = eventParams.StepID,
                            TaskID = eventParams.TaskID,
                            NextTasks = reslut.NextTasks.ToList(),

                        };

                        //步骤提交后事件
                        if (!step.Event.SubmitAfter.IsNullOrEmpty() &&
                            (execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit
                            || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed))
                        {
                            object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.SubmitAfter.Trim(), aftereventParams);
                            // Response.Write(string.Format("执行步骤提交后事件：({0}) 返回值：{1}<br/>", step.Event.SubmitAfter.Trim(), obj.ToString()));
                        }
                        if ((execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed || execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Submit) && step.Behavior.IsCopyForSend)
                        {
                            _workFlowTaskManager.WrokFlowTaskCopyFor(currentTask, step, execute, step.ID, 0, 2);
                        }
                        //步骤退回后事件
                        if (!step.Event.BackAfter.IsNullOrEmpty() && execute.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Back)
                        {
                            object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.BackAfter.Trim(), aftereventParams);
                            //  Response.Write(string.Format("执行步骤退回后事件：({0}) 返回值：{1}<br/>", step.Event.BackAfter.Trim(), obj.ToString()));
                        }
                    }
                }
            }

            //归档  暂时不实现
            if (reslut.IsSuccess)
            {
                //任务执行完后，把自动结束步骤往下走




                //判断是打开任务还是关闭窗口
                var nextTasks =
                    reslut.NextTasks.Where(
                        p => p.Status.In(0, 1) && p.ReceiveID == doUserId);
                var nextTask = nextTasks.Any() ? nextTasks.First() : null;
                if (nextTask != null)
                {
                    //hq:判断子流程进入连接
                    if (nextTask.Type == 6)
                    {
                        CurrentUnitOfWork.SaveChanges();
                        var childTask = _workFlowTaskRepository.GetAll().Where(r => r.GroupID == nextTask.SubFlowGroupID.ToGuid() && r.Status.In(0, 1) && r.ReceiveID == AbpSession.UserId.Value);
                        if (childTask.Any())
                        {
                            nextTask = childTask.First();
                            resultOutput.NextStepUrl = string.Format("/dynamicpage?fid={0}&stepID={1}&taskID={2}&groupID={3}&instanceID={4}", nextTask.FlowID, nextTask.StepID, nextTask.Id, nextTask.GroupID, nextTask.InstanceID);
                            // await UpdateOpenTime(nextTask.Id,nextTask.FlowID,nextTask.StepID);
                        }
                        else
                            resultOutput.NextStepUrl = "";
                    }
                    else
                    {
                        //await CurrentUnitOfWork.SaveChangesAsync();
                        //await UpdateOpenTime(nextTask.Id, nextTask.FlowID, nextTask.StepID);
                        resultOutput.NextStepUrl = string.Format("/dynamicpage?fid={0}&stepID={1}&taskID={2}&groupID={3}&instanceID={4}", nextTask.FlowID, nextTask.StepID, nextTask.Id, nextTask.GroupID, nextTask.InstanceID);
                    }
                }
                else
                {
                    resultOutput.NextStepUrl = "";
                }
                if (opation == "completed" && reslut.NextTasks != null && reslut.NextTasks.Count() > 0)
                {
                    var nsteps = new List<GetNextStepOutput>();
                    foreach (var task in reslut.NextTasks)
                    {
                        var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID, task.VersionNum);
                        var step = workflowModel.Steps.FirstOrDefault(r => r.ID == task.StepID);
                        var entityStep = new GetNextStepOutput();
                        var selectType = "";
                        var selectRang = "";
                        var defaultMember = _workFlowTaskManager.GetDefultMember(task.FlowID, step.ID, task.GroupID, step.ID, task.InstanceID, out selectType, out selectRang,
                            input.TaskId, task.VersionNum);
                        string[] array = defaultMember.Split(',');
                        var list = array.Where(x => x.StartsWith(MemberPerfix.UserPREFIX)).Select(x => Convert.ToInt64(x.Replace(MemberPerfix.UserPREFIX, ""))).ToList();
                        foreach (var item in list)
                        {
                            var dt = DateTime.Now;
                            var model = _roleRelationRepository.GetAll().FirstOrDefault(x => x.UserId == item && x.StartTime < dt && x.EndTime > dt);
                            if (model != null && !list.Contains(model.RelationUserId))
                            {
                                defaultMember = defaultMember.Replace(MemberPerfix.UserPREFIX + model.UserId, MemberPerfix.UserPREFIX + model.RelationUserId);
                            }
                        }
                        entityStep.DefaultUserId = defaultMember;
                        entityStep.DefaultUserName = organizeManager.GetNames(defaultMember);
                        entityStep.NextStepId = step.ID;
                        entityStep.NextStepName = step.Name;
                        entityStep.IsAllowChoose = step.Behavior.RunSelect != 0;
                        entityStep.SelectRangeRootId = step.Behavior.SelectRange?.Trim() ?? "";
                        nsteps.Add(entityStep);
                    }
                    resultOutput.Steps = nsteps;
                }
            }
            resultOutput.IsSuccesefull = reslut.IsSuccess;
            resultOutput.ReturnMsg = msg;
            if (!resultOutput.IsSuccesefull && resultOutput.ErrorMsg.IsNullOrWhiteSpace() && !resultOutput.ReturnMsg.IsNullOrWhiteSpace())
                resultOutput.ErrorMsg = resultOutput.ReturnMsg;
            return resultOutput;
        }



        public async Task<ExecuteWorkFlowOutput> FlowCopyForAsync(FlowCopyForInput input)
        {
            var currentTaskModel = await _workFlowTaskRepository.GetAsync(input.TaskId);
            var resultOutput = new ExecuteWorkFlowOutput();
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, currentTaskModel.VersionNum);
            if (workflowRunModel == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到流程";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var steps = workflowRunModel.Steps.Where(p => p.ID == input.StepId);
            if (steps.Count() == 0)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到当前步骤";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }

            var currentTask = await _workFlowTaskRepository.GetAsync(input.TaskId);
            if (currentTask == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "当前任务为空,请先保存再抄送!";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
            var tasks = workFlowTaskManager.GetTaskList2(currentTask.Id);
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = organizeManager.GetAllUsers(input.UserIds);
            System.Text.StringBuilder names = new System.Text.StringBuilder();
            foreach (var user in users)
            {
                if (tasks.Find(p => p.ReceiveID == user.Id) != null)
                {
                    continue;
                }
                var nextStep = workflowRunModel.Steps.Where(p => p.ID == input.StepId).First();
                var task = new WorkFlowTask();
                if (nextStep.WorkTime > 0)
                {
                    task.CompletedTime = DateTime.Now.AddHours((double)nextStep.WorkTime);
                }
                task.FlowID = currentTask.FlowID;
                task.GroupID = currentTask.GroupID;
                task.Id = Guid.NewGuid();
                task.Type = 5;
                task.InstanceID = currentTask.InstanceID;
                task.Note = "抄送任务";
                task.PrevID = currentTask.PrevID;
                task.PrevStepID = currentTask.PrevStepID;
                task.ReceiveID = user.Id;
                task.ReceiveName = user.Name;
                task.ReceiveTime = DateTime.Now;
                task.SenderID = currentTask.ReceiveID;
                task.SenderName = currentTask.ReceiveName;
                task.SenderTime = task.ReceiveTime;
                task.Status = 0;
                task.StepID = currentTask.StepID;
                task.StepName = "抄送";
                task.Sort = currentTask.Sort;
                task.Title = currentTask.Title;
                task.TodoType = currentTask.TodoType;
                task.Deepth = currentTask.Deepth;
                task.VersionNum = currentTask.VersionNum;
                await _workFlowTaskRepository.InsertAsync(task);

                // _roadFlowWorkFlowTaskRepository.Insert(task.MapToWorkFlowTask());
                names.Append(task.ReceiveName);
                names.Append(",");
            }
            _workFlowTaskManager.UpdateInstanceCopyForUsers(currentTask, users.Select(r => r.Id).ToList());
            await CurrentUnitOfWork.SaveChangesAsync();

            resultOutput.IsSuccesefull = true;
            resultOutput.ErrorMsg = $"成功抄送给：{names.ToString().TrimEnd(',')}";
            return resultOutput;
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<FlowInquiryOutput>> GetFlowInquiryList(GetFlowInquiryInput input)
        {
            var query = _workFlowTaskRepository.GetAll().Where(x => x.Type == 8 && x.FlowID == input.FlowId && x.PrevID == input.TaskId).OrderByDescending(x => x.SenderTime).GroupBy(x => x.InquiryGroupID);
            var list = new List<FlowInquiryOutput>();
            foreach (var item in query)
            {
                var items = item.OrderByDescending(x => x.CompletedTime1).ToList();
                var model = items.First();
                var info = new FlowInquiryOutput();
                info.Inquiry = model.Inquiry;
                info.SenderName = model.SenderName;
                info.ReceiveTime = model.ReceiveTime;
                info.Tasks = items.Select(x => new FlowInquiryTask() { Id = x.Id, Comment = x.Comment, CompletedTime1 = x.CompletedTime1, ReceiveName = x.ReceiveName }).ToList();
                list.Add(info);
            }
            return new PagedResultDto<FlowInquiryOutput>(query.Count(), list);
        }
        public async Task<ExecuteWorkFlowOutput> FlowInquiryAsync(FlowInquiryInput input)
        {
            var currentTaskModel = await _workFlowTaskRepository.GetAsync(input.TaskId);
            var resultOutput = new ExecuteWorkFlowOutput();
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, currentTaskModel.VersionNum);
            if (input.UserIds.IsNullOrEmpty())
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未指定征询人员";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.参数为空;
                return resultOutput;
            }
            if (workflowRunModel == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到流程";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var steps = workflowRunModel.Steps.Where(p => p.ID == input.StepId);
            if (steps.Count() == 0)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到当前步骤";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }

            var currentTask = await _workFlowTaskRepository.GetAsync(input.TaskId);
            if (currentTask == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "当前任务为空,请先保存再意见征询!";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
            var tasks = workFlowTaskManager.GetTaskList2(currentTask.Id);
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = organizeManager.GetAllUsers(input.UserIds);
            System.Text.StringBuilder names = new System.Text.StringBuilder();
            var guid = Guid.NewGuid();
            foreach (var user in users)
            {
                if (tasks.Find(p => p.ReceiveID == user.Id) != null)
                {
                    continue;
                }
                var nextStep = workflowRunModel.Steps.Where(p => p.ID == input.StepId).First();
                var task = new WorkFlowTask();
                if (nextStep.WorkTime > 0)
                {
                    task.CompletedTime = DateTime.Now.AddHours((double)nextStep.WorkTime);
                }
                task.FlowID = currentTask.FlowID;
                task.GroupID = currentTask.GroupID;
                task.InquiryGroupID = guid;
                task.Inquiry = input.Inquiry;
                task.Id = Guid.NewGuid();
                task.Type = 8;
                task.InstanceID = currentTask.InstanceID;
                task.Note = "意见征询";
                task.PrevID = currentTask.Id;
                task.PrevStepID = currentTask.PrevStepID;
                task.ReceiveID = user.Id;
                task.ReceiveName = user.Name;
                task.ReceiveTime = DateTime.Now;
                task.SenderID = currentTask.ReceiveID;
                task.SenderName = currentTask.ReceiveName;
                task.SenderTime = task.ReceiveTime;
                task.Status = 0;
                task.StepID = currentTask.StepID;
                task.StepName = currentTask.StepName;
                task.Sort = currentTask.Sort;
                task.Title = currentTask.Title;
                task.TodoType = currentTask.TodoType;
                task.Deepth = currentTask.Deepth;
                task.VersionNum = currentTask.VersionNum;
                await _workFlowTaskRepository.InsertAsync(task);

                // _roadFlowWorkFlowTaskRepository.Insert(task.MapToWorkFlowTask());
                names.Append(task.ReceiveName);
                names.Append(",");
            }
            await CurrentUnitOfWork.SaveChangesAsync();

            resultOutput.IsSuccesefull = true;
            resultOutput.ErrorMsg = $"成功向：{names.ToString().TrimEnd(',')}提交意见征询。";
            return resultOutput;
        }


        public ExecuteWorkFlowOutput FlowCopyFor(FlowCopyForInput input)
        {
            var currentTaskModel = _workFlowTaskRepository.Get(input.TaskId);
            var resultOutput = new ExecuteWorkFlowOutput();
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, currentTaskModel.VersionNum);
            if (workflowRunModel == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到流程";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var steps = workflowRunModel.Steps.Where(p => p.ID == input.StepId);
            if (steps.Count() == 0)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "未找到当前步骤";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }

            var currentTask = _workFlowTaskRepository.Get(input.TaskId);
            if (currentTask == null)
            {
                resultOutput.IsSuccesefull = false;
                resultOutput.ErrorMsg = "当前任务为空,请先保存再抄送!";
                resultOutput.ErrorType = ExecuteWorkFlowErrorType.未找到流程;
                return resultOutput;
            }
            var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
            var tasks = workFlowTaskManager.GetTaskList2(currentTask.Id);
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = organizeManager.GetAllUsers(input.UserIds);
            System.Text.StringBuilder names = new System.Text.StringBuilder();
            foreach (var user in users)
            {
                if (tasks.Find(p => p.ReceiveID == user.Id) != null)
                {
                    continue;
                }
                var nextStep = workflowRunModel.Steps.Where(p => p.ID == input.StepId).First();
                var task = new WorkFlowTask();
                if (nextStep.WorkTime > 0)
                {
                    task.CompletedTime = DateTime.Now.AddHours((double)nextStep.WorkTime);
                }
                task.FlowID = currentTask.FlowID;
                task.GroupID = currentTask.GroupID;
                task.Id = Guid.NewGuid();
                task.Type = 5;
                task.InstanceID = currentTask.InstanceID;
                task.Note = "抄送任务";
                task.PrevID = currentTask.PrevID;
                task.PrevStepID = currentTask.PrevStepID;
                task.ReceiveID = user.Id;
                task.ReceiveName = user.Name;
                task.ReceiveTime = DateTime.Now;
                task.SenderID = currentTask.ReceiveID;
                task.SenderName = currentTask.ReceiveName;
                task.SenderTime = task.ReceiveTime;
                task.Status = 0;
                task.StepID = currentTask.StepID;
                task.StepName = "抄送";
                task.Sort = currentTask.Sort;
                task.Title = currentTask.Title;
                task.Deepth = currentTask.Deepth;
                task.TodoType = currentTask.TodoType;
                task.VersionNum = currentTask.VersionNum;
                _workFlowTaskRepository.Insert(task);
                names.Append(task.ReceiveName);
                names.Append(",");
            }
            _workFlowTaskManager.UpdateInstanceCopyForUsers(currentTask, users.Select(r => r.Id).ToList());
            CurrentUnitOfWork.SaveChanges();
            resultOutput.IsSuccesefull = true;
            resultOutput.ErrorMsg = $"成功抄送给：{names.ToString().TrimEnd(',')}";
            return resultOutput;
        }



        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="addType">加签类型</param>
        /// <param name="writeType">审批类型</param>
        /// <param name="users">加签人员</param>
        /// <param name="note">加签备注</param>
        /// <returns></returns>
        public ExecuteWorkFlowOutput AddWrite(Guid taskID, int addType, int writeType, string writeUsers, string note)
        {
            var ret = new ExecuteWorkFlowOutput();
            var task = _workFlowTaskRepository.Get(taskID);
            if (task == null)
            {
                ret.ErrorMsg = "未找到当前任务,不能加签!";
                ret.ReturnMsg = "未找到当前任务,不能加签!";
                ret.IsSuccesefull = false;
                return ret;
            }
            #region 为选择的加签人员添加待办
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = organizeManager.GetAllUsers(writeUsers);
            int i = 0;
            foreach (var user in users)
            {
                var addTask = new WorkFlowTask();
                addTask.FlowID = task.FlowID;
                addTask.GroupID = task.GroupID;
                addTask.Id = Guid.NewGuid();
                addTask.InstanceID = task.InstanceID;
                addTask.Note = note;
                addTask.PrevID = task.Id;
                addTask.PrevStepID = task.PrevStepID;
                addTask.ReceiveID = user.Id;
                addTask.ReceiveName = user.Name;
                addTask.SenderTime = DateTime.Now;
                addTask.ReceiveTime = addTask.SenderTime.AddSeconds(i++);
                addTask.SenderID = task.ReceiveID;
                addTask.SenderName = task.ReceiveName;
                addTask.Sort = task.Sort + 1;
                addTask.StepID = task.StepID;
                addTask.StepName = task.StepName;
                addTask.Title = task.Title;
                addTask.Deepth = task.Deepth;
                addTask.VersionNum = task.VersionNum;
                addTask.OtherType = (addType.ToString() + writeType.ToString()).ToInt();
                addTask.Type = 7;
                if ((addType == 1 && writeType == 3 && user.Id != users.FirstOrDefault().Id) || addType == 2)
                {
                    addTask.Status = -1;
                }
                else
                {
                    addTask.Status = 0;
                }
                if (!_workFlowTaskManager.HasNoCompletedTasks(task.FlowID, task.StepID, task.GroupID, user.Id))
                {
                    //btask.Add(addTask);
                    _workFlowTaskRepository.Insert(addTask);
                }

            }
            #endregion
            ret.ReturnMsg = string.Join(',', users.Select(x => x.Name).ToList());
            #region 将当前任务的同级任务设置为等待中
            if (addType == 1)
            {
                var tjTasks = _workFlowTaskManager.GetTaskList2(taskID);
                foreach (var tjTask in tjTasks)
                {
                    tjTask.Status = -1;
                    _workFlowTaskRepository.Update(tjTask);
                }
            }
            #endregion
            ret.IsSuccesefull = true;
            return ret;
        }


        public ExecuteWorkFlowOutput AddWriteWithFlow(Guid taskId)
        {
            var taskModel = _workFlowTaskRepository.Get(taskId);
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(taskModel.FlowID, taskModel.VersionNum);
            var addType = 0;
            var writeType = 0;
            var addUsers = "";
            if (!GetInstanceAddWriteObject(flowModel, taskId, out addType, out writeType, out addUsers))
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "加签失败，参数获取异常");
            return AddWrite(taskId, addType, writeType, addUsers, "");
        }


        [UnitOfWork]
        public DefaultMemberModel GetDefaultMemberExecuteQuery(string sql)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDynamicRepository>();
            var ret = repository.Execute("UPDATE dbo.TestTable SET Status = 2");
            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "测试UnitOfWork");
            return _coreSqlQeuryRepository.SqlExecuteQuery(sql);

        }

        [AbpAuthorize]
        public async Task<PagedResultDto<WorkFlowTodoListDto>> GetTodoList(GetWorkFlowTodoListInput input)
        {
            var query = from task in _workFlowTaskRepository.GetAll()
                        join wf in _workFlowRepository.GetAll() on task.FlowID equals wf.Id
                        where task.ReceiveID == AbpSession.UserId.Value
                        select new
                        {
                            T = task,
                            W = wf,
                            NextTask = from b in _workFlowTaskRepository.GetAll()
                                       where b.PrevID == task.Id
                                       select new { b.Id, b.StepName, b.ReceiveName }

                        };

            var hasNoComplete = new int[] { 0, 1 };
            query = query.Where(r => r.T.Type != 6 && r.T.Status != 11);
            query = query.WhereIf(input.IsComplete == 0, r1 => hasNoComplete.Contains(r1.T.Status)).WhereIf(input.IsComplete == 1, r2 => r2.T.Status >= 2).WhereIf(!input.SearchKey.IsNullOrWhiteSpace(),
                r3 => r3.T.Title.Contains(input.SearchKey)).WhereIf(input.FlowId.HasValue, r4 => r4.T.FlowID == input.FlowId.Value);
            if (input.TaskType == 1) //项目类
            {
                query = query.Where(r5 => r5.W.Type == "CD897AD9-BC85-4D1E-85D7-08D5570CD07C".ToGuid());
            }
            else if (input.TaskType == 2)
            {
                query = query.Where(r6 => r6.W.Type != "CD897AD9-BC85-4D1E-85D7-08D5570CD07C".ToGuid());
            }
            var totalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r7 => r7.T.SenderTime).PageBy(input).ToListAsync();
            var retDatas = new List<WorkFlowTodoListDto>();
            foreach (var item in data)
            {
                var retEntity = new WorkFlowTodoListDto();
                var isHasten = false;

                retEntity.FlowID = item.T.FlowID;
                retEntity.FlowName = item.W.Name;
                retEntity.GroupID = item.T.GroupID;
                retEntity.ID = item.T.Id;
                retEntity.InstanceID = item.T.InstanceID;
                retEntity.Note = item.T.Note;
                retEntity.ReceiveTime = item.T.ReceiveTime;
                retEntity.ReceiveName = item.T.ReceiveName;
                retEntity.SenderName = item.T.SenderName;
                retEntity.SendTime = item.T.SenderTime;
                retEntity.StepID = item.T.StepID;
                retEntity.StepName = item.T.StepName;
                retEntity.Title = item.T.Title;
                retEntity.Status = item.T.Status;
                retEntity.StatusTitle = GetStatusTitle(item.T.Status);
                retEntity.OpenModel = input.IsComplete.ToString();
                if (input.IsComplete == 1)
                {
                    retEntity.CanCancle = _workFlowTaskManager.HasWithdraw(item.T.Id, out isHasten);
                    if (item.NextTask != null && item.NextTask.Count() > 0)
                    {
                        var nextRecevieNameList = item.NextTask.GroupBy(r => r.StepName).Select(r => new { StepName = r.Key, ReceiveName = r.Select(o => o.ReceiveName) });

                        foreach (var nextTask in nextRecevieNameList)
                        {
                            if (!retEntity.NextReciveName.IsNullOrWhiteSpace())
                                retEntity.NextReciveName = retEntity.NextReciveName + ",";
                            retEntity.NextReciveName = retEntity.NextReciveName + $"{nextTask.StepName}:{string.Join(",", nextTask.ReceiveName)}";
                        }
                    }
                }

                var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(item.T.FlowID, item.T.VersionNum);
                var step = wfInstalled.Steps.SingleOrDefault(ite => ite.ID == item.T.StepID);
                if (step == null)
                {
                    Abp.Logging.LogHelper.Logger.Error($"任务id：{item.T.Id}  获取stepid:{item.T.StepID}为空");
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据异常");
                }
                retEntity.WorkFlowModelId = step.WorkFlowModelId;

                retDatas.Add(retEntity);

            }
            var ret = new PagedResultDto<WorkFlowTodoListDto>(totalCount, retDatas.ToList());
            return ret;
        }


        /// <summary>
        /// 收回任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task WithdrawTask(Guid taskId)
        {
            _workFlowTaskManager.WithdrawTask(taskId);
            // 更新业务表状态
            var task = _workFlowTaskRepository.Get(taskId);
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID, task.VersionNum);
            var step = workflowRunModel.Steps.FirstOrDefault(r => r.ID == task.StepID);
            await UpdateInstanceStatusAsync(workflowRunModel, step.StepToStatus, task.Id, task.FlowID, task.InstanceID);
            //处理收回自定义方法       
            if (step.Event != null && !step.Event.Withdraw.IsNullOrEmpty())
            {
                var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
                var eventParams = new WorkFlowCustomEventParams();
                eventParams.FlowID = task.FlowID;
                eventParams.GroupID = task.GroupID;
                eventParams.StepID = task.StepID;
                eventParams.TaskID = task.Id;
                eventParams.InstanceID = task.InstanceID;
                object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.Withdraw.Trim(), eventParams);
            }
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<WorkTaskList>> GetWorkTaskPage(GetWorkTaskListInput input)
        {
            var all = new List<WorkTaskList>();

            var query = from p in _workFlowTaskRepository.GetAll().Where(p => p.InstanceID == input.ProjectId)
                        select new WorkTaskList
                        {
                            Id = p.Id,
                            TaskType = p.Type,
                            Title = p.StepName,
                            CreationTime = p.SenderTime,
                            UserName = p.SenderName,
                            ReceiveName = p.ReceiveName,
                            CompletedTime = p.CompletedTime1 == null ? "" : p.CompletedTime1.ToString(),
                            StatusTitle = GetStatusTitle(p.Status),
                            Note = p.Note,
                            Comment = p.Comment,
                        };
            var total = await query.CountAsync();
            if (total == 0)
                return new PagedResultDto<WorkTaskList>(0, new List<WorkTaskList>());
            var list = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in list)
            {
                var fileModel = await _abpFileRelationAppService.GetAsync(new GetAbpFilesInput()
                {
                    BusinessId = item.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.流程意见
                });
                item.CommentFile = fileModel;
            }
            return new PagedResultDto<WorkTaskList>(total, list);
        }



        public async Task<List<WorkFlowTaskCommentDto>> GetInstanceCommentAsync(GetWorkFlowTaskCommentInput input)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FlowID", input.FlowId);
            parameters.Add("@InstanceID", input.InstanceId);
            var data = await _dynamicRepository.QueryAsync<WorkFlowTask>($"exec [dbo].[spGetWorkFlowTaskComment] @FlowID,@InstanceID", parameters);
            var ret = new List<WorkFlowTaskCommentDto>();
            if (data.Count() == 0) return ret;
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            foreach (var item in data)
            {
                var stepModel = workflowRunModel.Steps.FirstOrDefault(r => r.ID == item.StepID);
                if (stepModel == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在此步骤");

                if (stepModel.OpinionDisplay == 1)
                {
                    var entity = new WorkFlowTaskCommentDto();
                    entity.StepId = stepModel.ID;
                    entity.StepName = stepModel.Name;
                    entity.SugguestionTitle = stepModel.SugguestionTitle;
                    entity.AddTime = item.CompletedTime1.Value;
                    entity.Comment = item.Comment;
                    if (stepModel.SignatureType == 2)
                    {
                        var signFileId = _employeesSignManager.GetSignFileId(item.ReceiveID);
                        if (signFileId.HasValue)
                            entity.SignFileId = signFileId.ToString();
                    }
                    entity.RecevieUserName = item.ReceiveName;
                    ret.Add(entity);
                }
            }

            return ret;
        }
        public List<WorkFlowTaskStepComentResult> GetCurrentUserComents(GetWorkFlowTaskCommentInput input)
        {
            return _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == input.InstanceId && x.ReceiveID == AbpSession.UserId.Value && x.Status == 1).Select(x => new WorkFlowTaskStepComentResult() { TaskId = x.Id, Comment = x.Comment }).ToList();
        }
        public List<WorkFlowTaskStepFileResult> GetInstanceFiles(GetWorkFlowTaskCommentInput input)
        {
            var data = _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == input.InstanceId);
            var ret = new List<WorkFlowTaskStepFileResult>();
            if (data.Count() == 0) return ret;
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, data.FirstOrDefault().VersionNum);
            var groupData = data.GroupBy(x => x.StepID).ToList();
            foreach (var item in groupData)
            {
                var stepModel = workflowRunModel.Steps.FirstOrDefault(r => r.ID == item.Key);
                if (stepModel == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在此步骤");
                var entity = new WorkFlowTaskStepFileResult();
                entity.StepID = stepModel.ID;
                entity.StepName = stepModel.Name;
                var list = item.ToList();
                var fileList = new List<GetAbpFilesTaskOutput>();
                foreach (var info in list)
                {
                    fileList.AddRange(_abpFileRelationAppService.GetList(new GetAbpFilesInput()
                    {
                        BusinessId = info.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.流程意见
                    }).Select(x => new GetAbpFilesTaskOutput()
                    {
                        Id = x.Id,
                        FileName = x.FileName,
                        FileSize = x.FileSize,
                        Sort = x.Sort,
                        TaskId = info.Id.ToString()
                    }));
                }
                entity.FileList = fileList;
                if (entity.FileList.Count() > 0)
                    ret.Add(entity);
            }
            return ret;
        }
        public List<WorkFlowTaskCommentDto> GetInstanceComment(GetWorkFlowTaskCommentInput input)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FlowID", input.FlowId);
            parameters.Add("@InstanceID", input.InstanceId);
            var data = _dynamicRepository.Query<WorkFlowTask>($"exec [dbo].[spGetWorkFlowTaskComment] @FlowID,@InstanceID", parameters);
            var ret = new List<WorkFlowTaskCommentDto>();
            if (data.Count() == 0) return ret;
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowRunModel = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, data.FirstOrDefault().VersionNum);
            foreach (var item in data)
            {
                var stepModel = workflowRunModel.Steps.FirstOrDefault(r => r.ID == item.StepID);
                if (stepModel == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在此步骤");

                if (stepModel.OpinionDisplay == 1)
                {
                    var entity = new WorkFlowTaskCommentDto();
                    entity.StepId = stepModel.ID;
                    if (item.Type == 5)
                        entity.StepName = item.StepName;
                    else
                        entity.StepName = stepModel.Name;
                    //entity.StepName = item.StepName;
                    entity.SugguestionTitle = stepModel.SugguestionTitle;
                    entity.AddTime = item.CompletedTime1.Value;
                    entity.Comment = item.Comment;
                    if (stepModel.SignatureType == 2)
                    {
                        var signFileId = _employeesSignManager.GetSignFileId(item.ReceiveID);
                        if (signFileId.HasValue)
                            entity.SignFileId = signFileId.ToString();
                    }
                    entity.SignFileId = item.SignFileId.ToString();
                    entity.RecevieUserName = item.ReceiveName;
                    entity.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
                    {
                        BusinessId = item.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.流程意见
                    });
                    ret.Add(entity);
                }

            }

            return ret;
        }

        /// <summary>
        /// 更新打开时间；点击处理链接后是否改变task状态为1； 项目评审 oa任务执行  不需要更改；其它需要改
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId">步骤id</param>
        public async Task UpdateOpenTime(Guid taskid, Guid flowId, Guid stepId)
        {
            var model = await _workFlowTaskRepository.GetAsync(taskid);
            if (!model.OpenTime.HasValue)
            {
                model.OpenTime = DateTime.Now;

            }
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            var stepModel = flowModel.Steps.FirstOrDefault(r => r.ID == stepId);
            if (stepModel != null)
            {
                if (stepModel.ID.ToString() == "82B42CCC-18EF-4234-8C5D-C17A3A9FE5A2" || stepModel.ID.ToString() == "ADC64038-C8BA-41D8-86E7-FC7CE348BD7C")
                {

                }
                else
                {
                    model.Status = 1;
                }

            }

        }


        /// <summary>
        /// 终止一个任务所在的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task EndTask(EndTaskInput input)
        {
            var task = await _workFlowTaskRepository.GetAsync(input.TaskId);
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前任务");
            }
            var tasks = _workFlowTaskManager.GetTaskList3(task.FlowID, task.GroupID);
            foreach (var t in tasks)
            {
                if (t.Status < 2)
                {
                    t.Status = t.Id == task.Id ? 6 : 7;
                    if (t.Id == task.Id)
                    {
                        t.Comment = input.Comment;
                        t.CompletedTime1 = DateTime.Now;
                        await _workFlowTaskRepository.UpdateAsync(t);

                        var fileList = new List<AbpFileListInput>();
                        if (input.FileList != null)
                        {
                            foreach (var item in input.FileList)
                            {
                                fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                            }
                        }
                        await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                        {
                            BusinessId = t.Id.ToString(),
                            BusinessType = (int)AbpFileBusinessType.流程意见,
                            Files = fileList
                        });
                    }
                }
            }



            #region 如果该任务是子流程任务则要判断是否应该提交主流程步骤
            var subTask = _workFlowTaskManager.GetTaskList3(Guid.Empty, task.GroupID).Find(p => p.OtherType == 4);
            if (subTask != null)
            {
                var mainTasks = _workFlowTaskManager.GetBySubFlowGroupID(subTask.GroupID);
                bool subFlowIsCompleted = true;
                foreach (var mainTask in mainTasks)
                {
                    if (!subFlowIsCompleted)
                    {
                        break;
                    }
                    foreach (string subFlowGroupID in mainTask.SubFlowGroupID.Split(','))
                    {
                        if (!_workFlowTaskManager.GetInstanceIsCompletedWithOutCopyTask(subTask.FlowID, subFlowGroupID.ToGuid()))
                        {
                            subFlowIsCompleted = false;
                            break;
                        }
                    }
                }
                if (subFlowIsCompleted)
                {
                    foreach (var mainTask in mainTasks)
                    {
                        var autiSubmitResult = _workFlowTaskManager.AutoSubmit(mainTask, true);
                        if (!autiSubmitResult.IsSuccess)
                        {
                            //result = autiSubmitResult;
                            //return;
                            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, autiSubmitResult.Messages);
                        }
                    }
                }
            }
            #endregion


            #region 更新业务表标识字段的值为-2
            var businessStatus = -2;
            if (input.IsStopNotNoPass)
                businessStatus = -4;
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var wfInstalled = workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID);
            if (wfInstalled.TitleField != null && wfInstalled.TitleField.LinkID != Guid.Empty && !wfInstalled.TitleField.Table.IsNullOrEmpty()
                && !wfInstalled.TitleField.Field.IsNullOrEmpty() && wfInstalled.DataBases.Count() > 0)
            {
                var firstDB = wfInstalled.DataBases.First();
                string sql = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3}", wfInstalled.TitleField.Table, wfInstalled.TitleField.Field,
               businessStatus, string.Format("{0}='{1}'", firstDB.PrimaryKey, task.InstanceID));
                _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                if (input.IsStopNotNoPass)
                {
                    _workFlowTaskManager.CreateNoticeForTask(task.FlowID, task.InstanceID, task.Title + "终止任务", "终止任务");
                }
            }
            #endregion

            #region 执行驳回自定义事件

            var step = wfInstalled.Steps.First(ite => ite.ID == task.StepID);
            if (step.Event != null && string.IsNullOrWhiteSpace(step.Event.EndTaskAfter) == false)
            {
                var workFlowTaskManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
                var eventParams = new WorkFlowCustomEventParams();
                eventParams.FlowID = task.FlowID;
                eventParams.GroupID = task.GroupID;
                eventParams.StepID = task.StepID;
                eventParams.TaskID = task.Id;
                eventParams.InstanceID = task.InstanceID;
                object obj = workFlowTaskManager.ExecuteFlowCustomEvent(step.Event.EndTaskAfter.Trim(), eventParams);
            }
            #endregion
        }


        public string GetInstanceCreatUserId(Guid flowId, string instanceId, string taskId)
        {
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowModel = workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            var firstDB = workflowModel.DataBases.First();
            try
            {
                string sql = $"select CreatorUserId   FROM  {workflowModel.TitleField.Table} WHERE {firstDB.PrimaryKey}=\'{instanceId}\'";
                var ret = _dynamicRepository.QueryFirst(sql);
                return $"u_{ret.CreatorUserId}";
            }
            catch (Exception err)
            {

                Abp.Logging.LogHelper.Logger.Error($"获取实例创建者失败 ({workflowModel.Name}),flowid:{flowId},taskid:{taskId},errormsg:{err.Message}");
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取实例创建者失败");
            }
        }



        private bool GetInstanceAddWriteObject(WorkFlowInstalled workflowModel, Guid taskId, out int addType, out int writeType, out string addUsers)
        {
            var isSuccess = false;
            addType = 0;
            writeType = 0;
            addUsers = "";
            var taskModel = _workFlowTaskRepository.Get(taskId);
            if (workflowModel.TitleField != null && workflowModel.TitleField.LinkID != Guid.Empty && !workflowModel.TitleField.Table.IsNullOrEmpty()
                && !workflowModel.TitleField.Field.IsNullOrEmpty() && workflowModel.DataBases.Any())
            {
                var firstDB = workflowModel.DataBases.First();
                try
                {
                    string sql = $"select AddType,WriteType,AddWriteUsers   FROM  {workflowModel.TitleField.Table} WHERE {firstDB.PrimaryKey}=\'{taskModel.InstanceID}\'";
                    var ret = _dynamicRepository.QueryFirst(sql);
                    addType = ret.AddType;
                    writeType = ret.WriteType;
                    addUsers = ret.AddWriteUsers;
                    isSuccess = true;
                }
                catch (Exception err)
                {

                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                    Abp.Logging.LogHelper.Logger.Error($"获取流程加签信息失败 ({workflowModel.Name}),flowid:{taskModel.FlowID},taskid:{taskModel.Id},errormsg:{err.Message}");
                }
            }
            return isSuccess;
        }


        /// <summary>
        /// 得到状态显示标题
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetStatusTitle(int status)
        {
            string title = string.Empty;
            switch (status)
            {
                case -1:
                    title = "等待中";
                    break;
                case 0:
                    title = "待处理";
                    break;
                case 1:
                    title = "处理中";
                    break;
                case 2:
                    title = "已完成";
                    break;
                case 3:
                    title = "已退回";
                    break;
                case 4:
                    title = "他人已处理";
                    break;
                case 5:
                    title = "他人已退回";
                    break;
                case 6:
                    title = "终止";
                    break;
                case 7:
                    title = "他人已终止";
                    break;
                case 8:
                    title = "退回审核";
                    break;
                case 9:
                    title = "申请停滞";
                    break;
                default:
                    title = "删除";
                    break;
            }

            return title;
        }

        private async Task UpdateInstanceStatusAsync(WorkFlowInstalled workflowModel, int toStatus, Guid taksId, Guid flowId, string instanceId)
        {
            #region 更新业务表标识字段的值为StepToStatus
            if (workflowModel.TitleField != null && workflowModel.TitleField.LinkID != Guid.Empty && !workflowModel.TitleField.Table.IsNullOrEmpty()
                && !workflowModel.TitleField.Field.IsNullOrEmpty() && workflowModel.DataBases.Any())
            {
                var firstDB = workflowModel.DataBases.First();
                try
                {
                    string sql = $"UPDATE {workflowModel.TitleField.Table} SET {workflowModel.TitleField.Field}=\'{toStatus}\' WHERE {firstDB.PrimaryKey}=\'{instanceId}\'";
                    _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                }
                catch (Exception err)
                {

                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                    Abp.Logging.LogHelper.Logger.Error($"更新流程状态迁移发生了错误({workflowModel.Name}),flowid:{flowId},taskid:{taksId},errormsg:{err.Message}");
                    throw err;
                }
            }
            #endregion

        }
        private void UpdateInstanceStatus(WorkFlowInstalled workflowModel, int toStatus, Guid taksId, Guid flowId, string instanceId)
        {
            #region 更新业务表标识字段的值为StepToStatus
            if (workflowModel.TitleField != null && workflowModel.TitleField.LinkID != Guid.Empty && !workflowModel.TitleField.Table.IsNullOrEmpty()
                && !workflowModel.TitleField.Field.IsNullOrEmpty() && workflowModel.DataBases.Any())
            {
                var firstDB = workflowModel.DataBases.First();
                try
                {
                    string sql = $"UPDATE {workflowModel.TitleField.Table} SET {workflowModel.TitleField.Field}=\'{toStatus}\' WHERE {firstDB.PrimaryKey}=\'{instanceId}\'";
                    _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                }
                catch (Exception err)
                {

                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                    Abp.Logging.LogHelper.Logger.Error($"更新流程状态迁移发生了错误({workflowModel.Name}),flowid:{flowId},taskid:{taksId},errormsg:{err.Message}");
                    throw err;
                }
            }
            #endregion

        }






        private ExecuteWorkFlowOutput ValidationExecute(ExecuteWorkFlowInput input)
        {
            var ret = new ExecuteWorkFlowOutput();
            if (input.FlowId == "056ae20f-8b23-4cdc-ae70-0c6ee935b066".ToGuid() && input.StepId == "eb563f43-b9a8-4cc1-99bc-5c9b0a0927b4".ToGuid())
            {
                var obj = _workFlowTaskManager.ExecuteFlowCustomEventByReflection("$YPCustEvent.IsComplateApply", new WorkFlowCustomEventParams() { FlowID = input.FlowId, InstanceID = input.InstanceId });
                var flag = (bool)obj;
                ret.IsSuccesefull = flag;
            }
            else
                ret.IsSuccesefull = true;
            return ret;
        }




    }
}
