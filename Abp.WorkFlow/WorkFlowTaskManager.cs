using Abp.Application.Services;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Reflection.Extensions;
using Castle.Windsor;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.UI;
using ZCYX.FRMSCore.Model;
using Abp.File;
using Abp.Events.Bus;

namespace Abp.WorkFlow
{
    [RemoteService(IsEnabled = false)]
    public class WorkFlowTaskManager : ApplicationService
    {
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<User, long> _useRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private WorkFlowInstalled wfInstalled;
        private WorkFlowResult result;
        private List<WorkFlowTask> nextTasks = new List<WorkFlowTask>();
        public IEventBus _eventBus { get; set; }
        private readonly ProjectNoticeManager _noticeManager;
        private readonly EmployeesSignManager _employeesSignManager;
        private readonly IRepository<TaskManagementRelation, Guid> _taskManagementRelation;
        public WorkFlowTaskManager(IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowCacheManager workFlowCacheManager, IRepository<User, long> useRepository
            , IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , IDynamicRepository dynamicRepository, IAbpFileRelationAppService abpFileRelationAppService, EmployeesSignManager employeesSignManager, ProjectNoticeManager noticeManager, IRepository<TaskManagementRelation, Guid> taskManagementRelation)
        {
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _useRepository = useRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _dynamicRepository = dynamicRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _employeesSignManager = employeesSignManager;
            _noticeManager = noticeManager;
            _eventBus = NullEventBus.Instance;
            _taskManagementRelation = taskManagementRelation;
        }

        /// <summary>
        /// 变更事务通知
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void CreateNoticeForTask(Guid flowId, string instanceId, string title, string content)
        {
            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            if (workflowModel.TitleField != null && workflowModel.TitleField.LinkID != Guid.Empty && !workflowModel.TitleField.Table.IsNullOrEmpty()
    && !workflowModel.TitleField.Field.IsNullOrEmpty() && workflowModel.DataBases.Any())
            {
                var firstDB = workflowModel.DataBases.First();
                try
                {
                    var query_Sql = $"select DealWithUsers from {workflowModel.TitleField.Table} where  {firstDB.PrimaryKey}=\'{instanceId}\'";
                    var instanceDealWithUsers = _dynamicRepository.QueryFirst(query_Sql);
                    var users = "";
                    if (instanceDealWithUsers.DealWithUsers == null)
                    {
                        return;
                    }
                    else
                    {
                        var exite_UserIdStr = instanceDealWithUsers.DealWithUsers as string;
                        var exite_UserIds = exite_UserIdStr.Split(',');
                        foreach (var item in exite_UserIds)
                        {
                            if (item != AbpSession.UserId.Value.ToString())
                                users += item + ",";
                        }
                    }
                    users = users.TrimEnd(',');
                    if (!string.IsNullOrEmpty(users))
                        _noticeManager.CreateOrUpdateNotice(new NoticePublishInput()
                        {
                            Title = title,
                            Content = content,
                            NoticeUserIds = users,
                            NoticeType = 1
                        });
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 执行自定义方法  有问题   反射后 abp的DI  不可用
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public object ExecuteFlowCustomEventByReflection(string eventName, object eventParams, string dllName = "")
        {
            //eventName = "$CustEvent.GetIsNanBuProjectLeader";
            eventName = eventName.Replace("$", "");
            var coreAssemblyDirectoryPath = typeof(WorkFlowTaskManager).GetAssembly().GetDirectoryPathOrNull();
            //if (dllName.IsNullOrEmpty())
            //{
            //    dllName = eventName.Substring(0, eventName.IndexOf('.'));
            //}
            dllName = "ZCYX.FRMSCore.Web.Host";
            var filePath = $"{coreAssemblyDirectoryPath}\\{dllName}.dll";
            var assembly = System.Reflection.Assembly.LoadFile(filePath);
            eventName = $"{dllName}.{eventName}";
            string typeName = System.IO.Path.GetFileNameWithoutExtension(eventName);
            string methodName = eventName.Substring(typeName.Length + 1);
            Type type = assembly.GetType(typeName, true);
            Type[] pt = new Type[1];
            object obj = assembly.CreateInstance(typeName);

            var method = type.GetMethod(methodName);

            if (method != null)
            {
                var result = method.Invoke(obj, new object[] { eventParams });
                return result;
            }
            else
            {
                throw new MissingMethodException(typeName, methodName);
            }
        }

        public dynamic ExecuteFlowCustomEvent(string eventName, WorkFlowCustomEventParams eventParams)
        {
            if (eventName.StartsWith("$"))
            {
                return ExecuteFlowCustomEventByReflection(eventName, eventParams);
            }
            else
            {
                return ExecuteFlowCustomEventByDapper(eventName, eventParams);
            }
        }


        public dynamic ExecuteFlowCustomEvent(string eventName, WorkFlowCustomEventParamsForAfterSubmit eventParams)
        {
            if (eventName.StartsWith("$"))
            {
                return ExecuteFlowCustomEventByReflection(eventName, eventParams);
            }
            else
            {
                return ExecuteFlowCustomEventByDapper(eventName, eventParams);
            }
        }

        public dynamic ExecuteFlowCustomEventByDapper(string eventName, WorkFlowCustomEventParams eventParams)
        {
            var repository = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowCustomEvent, Guid>>();
            var query = repository.GetAll().FirstOrDefault(r => r.Code == eventName);
            if (query == null) return null;
            var procedureName = query.ProcedureName;
            var repositoryDynamic = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDynamicRepository>();
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", eventParams.FlowID);
            parameters.Add("GroupID", eventParams.GroupID);
            parameters.Add("StepID", eventParams.StepID);
            parameters.Add("TaskID", eventParams.TaskID);
            parameters.Add("InstanceID", eventParams.InstanceID.IsNullOrEmpty() ? (object)DBNull.Value : eventParams.InstanceID);
            var ret = repositoryDynamic.QueryFirst($"exec {query.ProcedureName} @FlowID,@GroupID,@StepID,@TaskID,@InstanceID", parameters);
            return ret;
        }

        public dynamic ExecuteFlowCustomEventByDapperRetList(string eventName, WorkFlowCustomEventParams eventParams)
        {
            var repository = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowCustomEvent, Guid>>();
            var query = repository.GetAll().FirstOrDefault(r => r.Code == eventName);
            if (query == null) return null;
            var procedureName = query.ProcedureName;
            var repositoryDynamic = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDynamicRepository>();
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", eventParams.FlowID);
            parameters.Add("GroupID", eventParams.GroupID);
            parameters.Add("StepID", eventParams.StepID);
            parameters.Add("TaskID", eventParams.TaskID);
            parameters.Add("InstanceID", eventParams.InstanceID.IsNullOrEmpty() ? (object)DBNull.Value : eventParams.InstanceID);
            var ret = repositoryDynamic.Query($"exec {query.ProcedureName} @FlowID,@GroupID,@StepID,@TaskID,@InstanceID", parameters);
            return ret;
        }

        public IEnumerable<dynamic> ExecuteFlowCustomEventReturnListByDapper(string eventName, WorkFlowCustomEventParams eventParams)
        {
            var repository = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowCustomEvent, Guid>>();
            var query = repository.GetAll().FirstOrDefault(r => r.Code == eventName);
            if (query == null) return null;
            var procedureName = query.ProcedureName;
            var repositoryDynamic = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDynamicRepository>();
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", eventParams.FlowID);
            parameters.Add("GroupID", eventParams.GroupID);
            parameters.Add("StepID", eventParams.StepID);
            parameters.Add("TaskID", eventParams.TaskID);
            parameters.Add("InstanceID", eventParams.InstanceID.IsNullOrEmpty() ? (object)DBNull.Value : eventParams.InstanceID);
            var ret = repositoryDynamic.Query($"exec {query.ProcedureName} @FlowID,@GroupID,@StepID,@TaskID,@InstanceID", parameters);
            return ret;
        }

        public SubWorkFlowExecuteResult SubActiveEventByEF(string eventName, WorkFlowCustomEventParams eventParams)
        {
            var repository = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowCustomEvent, Guid>>();
            var query = repository.GetAll().FirstOrDefault(r => r.Code == eventName);
            if (query == null) return null;

            var repositoryEF = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISubFlowActiveRepository>();
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", eventParams.FlowID);
            parameters.Add("GroupID", eventParams.GroupID);
            parameters.Add("StepID", eventParams.StepID);
            parameters.Add("TaskID", eventParams.TaskID);
            parameters.Add("InstanceID", eventParams.InstanceID.IsNullOrEmpty() ? (object)DBNull.Value : eventParams.InstanceID);
            var ret = repositoryEF.SqlExecuteQuery($"exec {query.ProcedureName} @FlowID,@GroupID,@StepID,@TaskID,@InstanceID", parameters);
            return ret;
        }



        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="executeModel">处理实体</param>
        /// <returns></returns>
        [UnitOfWork]
        public WorkFlowResult Execute(WorkFlowExecute executeModel, bool isSubFlow = false, User doUser = null)
        {
            result = new WorkFlowResult();
            nextTasks = new List<WorkFlowTask>();
            if (executeModel.FlowID == Guid.Empty)
            {
                result.DebugMessages = "流程ID错误";
                result.IsSuccess = false;
                result.Messages = "执行参数错误";
                return result;
            }

            wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(executeModel.FlowID, executeModel.VersionNum);
            if (wfInstalled == null)
            {
                result.DebugMessages = "未找到流程运行时实体";
                result.IsSuccess = false;
                result.Messages = "流程运行时为空";
                return result;
            }

            lock (executeModel.GroupID.ToString())
            {
                switch (executeModel.ExecuteType)
                {
                    case WorkFlowEnumType.WorkFlowExecuteType.Back:
                        executeBack(executeModel);
                        break;
                    case WorkFlowEnumType.WorkFlowExecuteType.Save:
                        executeSave(executeModel);
                        break;
                    case WorkFlowEnumType.WorkFlowExecuteType.Submit:
                    case WorkFlowEnumType.WorkFlowExecuteType.Completed:
                        executeSubmit(executeModel, isSubFlow, doUser);
                        break;
                    case WorkFlowEnumType.WorkFlowExecuteType.Redirect:
                        executeRedirect(executeModel);
                        break;
                    case WorkFlowEnumType.WorkFlowExecuteType.AddWrite:
                        executeAddWrite(executeModel);
                        break;
                    case WorkFlowEnumType.WorkFlowExecuteType.CopyforCompleted:
                        executeCopyforComplete(executeModel);
                        break;
                    case WorkFlowEnumType.WorkFlowExecuteType.InquiryCompleted:
                        executeInquiryComplete(executeModel);
                        break;
                    default:
                        result.DebugMessages = "流程处理类型为空";
                        result.IsSuccess = false;
                        result.Messages = "流程处理类型为空";
                        return result;
                }

                ///在switch执行方法里面  执行结果失败 则返回
                if (!result.IsSuccess)
                    return result;

                result.NextTasks = nextTasks;
                //添加消息
                //保存不发送站内消息
                if (executeModel.ExecuteType != WorkFlowEnumType.WorkFlowExecuteType.Save)
                {
                    //var shorMsg = new RoadFlow.Platform.ShortMessage();
                    //shorMsg.Delete(executeModel.TaskID.ToString(), 1);
                    DeleteShortMessage(executeModel.TaskID.ToString(), 1);
                    foreach (var task in result.NextTasks.Where(p => p.Status.In(0)))
                    {
                        if (!(task.ReceiveID == 0))
                        {
                            //下一步处理人是自己不发站内消息
                            if (task.ReceiveID == task.SenderID)
                            {
                                continue;
                            }
                            string url = "";
                            // zcl  暂注释掉处理 全部转到/WorkFlowRun/Index
                            //if (System.Web.HttpContext.Current.Request.Url != null
                            //    && System.Web.HttpContext.Current.Request.Url.AbsolutePath.EndsWith(".aspx", StringComparison.CurrentCultureIgnoreCase))
                            //{
                            //    url = "/Platform/WorkFlowRun/Default.aspx";
                            //}
                            //else
                            //{
                            //    url = "/WorkFlowRun/Index";
                            //}
                            url = "/WorkFlowRun/Index";
                            Guid msgID = Guid.NewGuid();
                            //string linkUrl = "javascript:openApp('" + url + "?flowid=" + task.FlowID + "&stepid=" + task.StepID + "&instanceid=" + task.InstanceID + "&taskid=" + task.Id + "&groupid=" + task.GroupID + "',0,'" + task.Title.Replace1(",", "") + "','tab_" + task.Id + "');closeMessage('" + msgID + "');";
                            string linkUrl = $"/dynamicpage?fid={task.FlowID}&instanceID={task.InstanceID}&stepID={task.StepID}&groupID={task.GroupID}&taskID={task.Id}";
                            result.Send.Add(new WorkFlowNoticeMessage
                            {
                                RecieveId = task.ReceiveID,
                                Title = "流程待办提醒",
                                Content = "您有一个新的待办任务，流程:" + wfInstalled.Name + "，步骤：" + task.StepName + "。",
                                LinkUrl = linkUrl
                            });

                            //to do
                            //  ShortMessage.Send(task.ReceiveID, task.ReceiveName, "流程待办提醒", "您有一个新的待办任务，流程:" + wfInstalled.Name + "，步骤：" + task.StepName + "。", 1, linkUrl, task.ID.ToString(), msgID.ToString());
                        }
                    }
                }
                return result;
            }
        }






        /// <summary>
        /// 退回任务
        /// </summary>
        /// <param name="executeModel"></param>
        [UnitOfWork]
        private void executeBack(WorkFlowExecute executeModel)
        {
            var currentTask = _workFlowTaskRepository.Get(executeModel.TaskID);
            var currentUser = _useRepository.Get(AbpSession.UserId.Value);
            if (currentTask == null)
            {
                result.DebugMessages = "未能找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未能找到当前任务";
                return;
            }
            else if (currentTask.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            else if (currentTask.ReceiveID != currentUser.Id)
            {
                result.DebugMessages = "不能处理当前任务";
                result.IsSuccess = false;
                result.Messages = "不能处理当前任务";
                return;
            }

            var currentSteps = wfInstalled.Steps.Where(p => p.ID == currentTask.StepID);
            var currentStep = currentSteps.Any() ? currentSteps.First() : null;

            if (currentStep == null)
            {
                result.DebugMessages = "未能找到当前步骤";
                result.IsSuccess = false;
                result.Messages = "未能找到当前步骤";
                return;
            }
            if (currentTask.StepID == wfInstalled.FirstStepID)
            {
                result.DebugMessages = "当前任务是流程第一步,不能退回";
                result.IsSuccess = false;
                result.Messages = "当前任务是流程第一步,不能退回";
                return;
            }
            if (executeModel.Steps.Count == 0)
            {
                result.DebugMessages = "没有选择要退回的步骤";
                result.IsSuccess = false;
                result.Messages = "没有选择要退回的步骤";
                return;
            }

            #region 加签退回
            if (currentTask.Type == 7 && currentTask.OtherType.HasValue)
            {
                int addType = currentTask.OtherType.Value.ToString().Left(1).ToInt();
                int writeType = currentTask.OtherType.Value.ToString().Right(1).ToInt();
                var tjTasks = GetTaskList1(currentTask.FlowID, currentTask.StepID, currentTask.GroupID).FindAll(p => p.PrevID == currentTask.PrevID && p.Type == 7);
                bool isBack1 = false;
                switch (writeType)
                {
                    case 1:
                    case 3:
                        foreach (var t in tjTasks)
                        {
                            if (t.Id == currentTask.Id)
                            {
                                Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                            }
                            else if (t.Status.In(-1, 0, 1))
                            {
                                t.Status = 5;
                                _workFlowTaskRepository.Update(t);
                            }
                        }
                        isBack1 = true;
                        break;
                    case 2:
                        Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                        if (tjTasks.FindAll(p => p.Status.In(-1, 0, 1) && p.Id != currentTask.Id).Count == 0)
                        {
                            isBack1 = true;
                        }
                        break;
                }

                if (isBack1)
                {
                    var prevTask = _workFlowTaskRepository.Get(currentTask.PrevID);
                    if (prevTask != null)
                    {
                        if (addType == 2)
                        {
                            foreach (var t in GetNextTaskList(prevTask.Id))
                            {
                                if (t.Status == -1)
                                {
                                    _workFlowTaskRepository.Delete(t.Id);
                                }
                            }
                        }
                        prevTask.Status = 0;
                        _workFlowTaskRepository.Update(prevTask);
                        nextTasks.Add(prevTask);
                        result.DebugMessages += "已退回到" + prevTask.ReceiveName;
                        result.IsSuccess = true;
                        result.Messages += result.DebugMessages;
                        result.NextTasks = nextTasks;
                    }
                    else
                    {
                        result.DebugMessages += "未找到前一任务";
                        result.IsSuccess = false;
                        result.Messages += result.DebugMessages;
                        result.NextTasks = nextTasks;
                    }
                }
                else
                {
                    result.DebugMessages += "已退回,等待他人处理";
                    result.IsSuccess = true;
                    result.Messages += result.DebugMessages;
                    result.NextTasks = nextTasks;
                }


                return;
            }
            #endregion

            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            //{
            var backTasks = new List<WorkFlowTask>();
            int status = 0;
            int backModel = currentStep.Behavior.BackModel;
            int hanlderModel = currentStep.Behavior.HanlderModel;
            if (backModel == 2)//一人退回全部退回
            {
                backModel = 1;
                hanlderModel = 0;
            }
            else if (backModel == 3)//所有人退回才退回
            {
                backModel = 1;
                hanlderModel = 1;
            }
            switch (backModel)
            {
                case 0://不能退回
                    result.DebugMessages = "当前步骤设置为不能退回";
                    result.IsSuccess = false;
                    result.Messages = "当前步骤设置为不能退回";
                    return;
                #region 根据策略退回
                case 1:
                    switch (hanlderModel)
                    {
                        case 0://所有人必须同意,如果一人不同意则全部退回
                            var taskList = GetTaskList1(currentTask.FlowID, currentTask.StepID, currentTask.GroupID).FindAll(p => p.Sort == currentTask.Sort && p.Type != 5);
                            foreach (var task in taskList)
                            {
                                if (task.Id != currentTask.Id)
                                {
                                    if (task.Status.In(0, 1))
                                    {
                                        Completed(task.Id, "", false, 5);
                                        //backTasks.Add(task);
                                    }
                                }
                                else
                                {
                                    Completed(task.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                                }
                            }
                            break;
                        case 1://一人同意即可
                            var taskList1 = GetTaskList1(currentTask.FlowID, currentTask.StepID, currentTask.GroupID).FindAll(p => p.Sort == currentTask.Sort && p.Type != 5);
                            if (taskList1.Count > 1)
                            {
                                var noCompleted = taskList1.Where(p => p.Status != 3);
                                if (noCompleted.Count() - 1 > 0)
                                {
                                    status = -1;
                                }
                            }
                            Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                            break;
                        case 2://依据人数比例
                            var taskList2 = GetTaskList1(currentTask.FlowID, currentTask.StepID, currentTask.GroupID).FindAll(p => p.Sort == currentTask.Sort && p.Type != 5);
                            if (taskList2.Count > 1)
                            {
                                currentStep.Behavior.Percentage = currentStep.Behavior.Percentage ?? 0;
                                decimal percentage = currentStep.Behavior.Percentage.Value <= 0 ? 100 : currentStep.Behavior.Percentage.Value;//比例
                                if ((((decimal)(taskList2.Count(p => p.Status == 3) + 1) / (decimal)taskList2.Count) * 100).Round() < 100 - percentage)
                                {
                                    status = -1;
                                }
                                else
                                {
                                    foreach (var task in taskList2)
                                    {
                                        if (task.Id != currentTask.Id && task.Status.In(0, 1))
                                        {
                                            Completed(task.Id, "", false, 5);
                                            //backTasks.Add(task);
                                        }
                                    }
                                }
                            }
                            Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                            break;
                        case 3://独立处理
                            Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                            break;
                    }
                    backTasks.Add(currentTask);
                    break;
                #endregion
                case 4: //独立退回

                    break;


            }

            if (status == -1)
            {
                result.DebugMessages += "已退回,等待他人处理";
                result.IsSuccess = true;
                result.Messages += "已退回,等待他人处理!";
                result.NextTasks = nextTasks;
                //scope.Complete();
                return;
            }

            foreach (var backTask in backTasks)
            {
                if (backTask.Status.In(2, 3))//已完成的任务不能退回
                {
                    continue;
                }
                if (backTask.Id == currentTask.Id)
                {
                    Completed(backTask.Id, executeModel.Comment, executeModel.IsSign, 3, files: executeModel.Files);
                }
                else
                {
                    Completed(backTask.Id, "", false, 5, "他人已退回");
                }
            }

            var tasks = new List<WorkFlowTask>();
            /* 因为设置为退回到第一步时，会同时退回上一步和第一步两个步骤，所以临时注释掉（2016-4-1）
            if (currentStep.Behavior.HanlderModel.In(0, 1, 2))//退回时要退回其它步骤发来的同级任务。
            {
                var tjTasks = GetTaskList(currentTask.FlowID, currentTask.StepID, currentTask.GroupID).FindAll(p => p.Sort == currentTask.Sort);
                foreach (var tjTask in tjTasks)
                {
                    if (!executeModel.Steps.ContainsKey(tjTask.PrevStepID))
                    {
                        executeModel.Steps.Add(tjTask.PrevStepID, new List<Data.Model.Users>());
                    }
                }
            }*/
            foreach (var step in executeModel.Steps)
            {
                var tasks1 = GetTaskList1(executeModel.FlowID, step.Key, executeModel.GroupID).FindAll(p => p.Type != 7);

                if (tasks1.Count > 0)
                {
                    tasks1 = tasks1.OrderByDescending(p => p.Sort).ToList();
                    int maxSort = tasks1.First().Sort;

                    ///当退回为独立处理的时候，  会给step的value赋值前一步的处理人员  其它退回策略时， Value的count为0 
                    if (step.Value.Count > 0)
                    {
                        tasks.AddRange(tasks1.FindAll(p => p.Sort == maxSort && step.Value.Select(r => r.Id).ToList().Contains(p.ReceiveID)));
                    }
                    else
                    {
                        tasks.AddRange(tasks1.FindAll(p => p.Sort == maxSort));
                    }


                }
            }

            #region 处理会签形式的退回
            //当前步骤是否是会签步骤
            var countersignatureStep = GetNextSteps(executeModel.FlowID, executeModel.StepID, executeModel.VersionNum).Find(p => p.Behavior.Countersignature != 0);
            bool IsCountersignature = countersignatureStep != null;
            bool isBack = true;
            if (IsCountersignature)
            {
                var steps = GetPrevSteps(executeModel.FlowID, countersignatureStep.ID);
                switch (countersignatureStep.Behavior.Countersignature)
                {
                    case 1://所有步骤处理，如果一个步骤退回则退回
                        isBack = false;
                        foreach (var step in steps)
                        {
                            if (IsBack(step, executeModel.FlowID, currentTask.GroupID, currentTask.PrevID, currentTask.Sort))
                            {
                                isBack = true;
                                break;
                            }
                        }
                        break;
                    case 2://一个步骤退回,如果有一个步骤同意，则不退回
                        isBack = true;
                        foreach (var step in steps)
                        {
                            if (!IsBack(step, executeModel.FlowID, currentTask.GroupID, currentTask.PrevID, currentTask.Sort))
                            {
                                isBack = false;
                                break;
                            }
                        }
                        break;
                    case 3://依据比例退回
                        int backCount = 0;
                        foreach (var step in steps)
                        {
                            if (IsBack(step, executeModel.FlowID, currentTask.GroupID, currentTask.PrevID, currentTask.Sort))
                            {
                                backCount++;
                            }
                        }
                        isBack = (((decimal)backCount / (decimal)steps.Count) * 100).Round() >= (countersignatureStep.Behavior.CountersignaturePercentage <= 0 ? 100 : countersignatureStep.Behavior.CountersignaturePercentage);
                        break;
                }

                if (isBack)
                {
                    var tjTasks = GetTaskList2(currentTask.Id, false);
                    foreach (var tjTask in tjTasks)
                    {
                        if (tjTask.Id == currentTask.Id || tjTask.Status.In(2, 3, 4, 5, 6, 7))
                        {
                            continue;
                        }
                        Completed(tjTask.Id, "", false, 5);
                    }
                }
            }
            #endregion

            //如果退回步骤是子流程步骤，则要作废子流程实例
            if (currentStep.Type == "subflow" && currentStep.SubFlowID.IsGuid() && !currentTask.SubFlowGroupID.IsNullOrEmpty())
            {
                foreach (string groupID in currentTask.SubFlowGroupID.Split(','))
                {
                    DeleteInstance(currentStep.SubFlowID.ToGuid(), groupID.ToGuid(), true);
                }
            }

            if (isBack)
            {
                var backTaskList = tasks.Distinct(new WorkTasksEqualityComparer()).ToList();
                if (!backTaskList.Any())
                {
                    Completed(currentTask.Id, "", false, 0, "");
                    result.DebugMessages += "没有接收人,退回失败!";
                    result.IsSuccess = false;
                    result.Messages += "没有接收人,退回失败!";
                    result.NextTasks = nextTasks;
                    //scope.Complete();
                    return;
                }

                foreach (var task in backTaskList)
                {
                    if (task != null)
                    {
                        //删除抄送
                        if (task.Type == 5)
                        {
                            _workFlowTaskRepository.Delete(task.Id);
                            continue;
                        }

                        if (task.OtherType == 1)
                        {
                            var prevtask = _workFlowTaskRepository.Get(task.PrevID);
                            if (prevtask != null)
                            {
                                prevtask.OpenTime = null;
                                prevtask.Status = 0;
                                _workFlowTaskRepository.Update(prevtask);
                                _workFlowTaskRepository.Delete(task.Id);
                                nextTasks.Add(prevtask);
                            }
                        }
                        else
                        {
                            var newTask = task;
                            newTask.Id = Guid.NewGuid();
                            newTask.PrevID = currentTask.Id;
                            newTask.Note = "退回任务";
                            newTask.ReceiveTime = DateTime.Now;
                            newTask.SenderID = currentTask.ReceiveID;
                            newTask.SenderName = currentTask.ReceiveName;
                            newTask.SenderTime = DateTime.Now;
                            newTask.Sort = currentTask.Sort + 1;
                            newTask.VersionNum = currentTask.VersionNum;
                            newTask.Status = 0;
                            if (currentStep.IsHideTask)
                                newTask.Status = -1;
                            newTask.Type = 4;
                            newTask.Comment = "";
                            newTask.OpenTime = null;
                            newTask.Deepth = (currentTask.Deepth + 1);
                            //newTask.PrevStepID = currentTask.StepID;
                            if (currentStep.WorkTime > 0)
                            {
                                newTask.CompletedTime = DateTime.Now.AddHours((double)currentStep.WorkTime);
                            }
                            else
                            {
                                newTask.CompletedTime = null;
                            }
                            newTask.CompletedTime1 = null;
                            _workFlowTaskRepository.Insert(newTask);
                            nextTasks.Add(newTask);
                            CreateNoticeForTask(currentTask.FlowID, currentTask.InstanceID, currentTask.Title + "退回任务", "退回任务");
                        }
                    }
                }

                //删除临时任务
                var nextSteps = GetNextSteps(executeModel.FlowID, executeModel.StepID, executeModel.VersionNum);
                foreach (var step in nextSteps)
                {
                    DeleteTempTasks(currentTask.FlowID, step.ID, currentTask.GroupID,
                        IsCountersignature ? Guid.Empty : currentStep.ID
                        );
                }
            }

            //    scope.Complete();
            //}

            if (nextTasks.Count > 0)
            {
                List<string> nextStepName = new List<string>();
                foreach (var nstep in nextTasks)
                {
                    nextStepName.Add(nstep.StepName);
                }

                string msg = $"已退回到:{nextStepName.Distinct().ToArray().Join1(",")}";
                result.DebugMessages += msg;
                result.IsSuccess = true;
                result.Messages += msg;
                result.NextTasks = nextTasks;
            }
            else
            {
                result.DebugMessages += "已退回,等待其它步骤处理";
                result.IsSuccess = true;
                result.Messages += "已退回,等待其它步骤处理";
                result.NextTasks = nextTasks;
            }
            return;
        }


        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="executeModel"></param>
        [UnitOfWork]
        private void executeSave(WorkFlowExecute executeModel)
        {
            //如果是第一步提交并且没有实例则先创建实例
            WorkFlowTask currentTask = null;
            var currentUser = _useRepository.Get(AbpSession.UserId.Value);
            bool isFirst = executeModel.StepID == wfInstalled.FirstStepID && executeModel.TaskID == Guid.Empty && executeModel.GroupID == Guid.Empty;
            if (isFirst)
            {

                currentTask = createFirstTask(executeModel);
            }
            else
            {
                currentTask = _workFlowTaskRepository.Get(executeModel.TaskID);
            }
            if (currentTask == null)
            {
                result.DebugMessages = "未能创建或找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未能创建或找到当前任务";
                return;
            }
            else if (currentTask.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            else if (currentTask.ReceiveID != currentUser.Id)
            {
                result.DebugMessages = "不能处理当前任务";
                result.IsSuccess = false;
                result.Messages = "不能处理当前任务";
                return;
            }
            else
            {
                currentTask.InstanceID = executeModel.InstanceID;
                nextTasks.Add(currentTask);
                if (isFirst)
                {
                    currentTask.Title = executeModel.Title.IsNullOrEmpty() ? "未命名任务" : executeModel.Title;
                    _workFlowTaskRepository.Update(currentTask);
                }
                else
                {
                    if (!executeModel.Title.IsNullOrEmpty())
                    {
                        currentTask.Title = executeModel.Title;
                        _workFlowTaskRepository.Update(currentTask);
                    }
                }
            }

            result.DebugMessages = "保存成功";
            result.IsSuccess = true;
            result.Messages = "保存成功";
        }


        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="executeModel"></param>
        [UnitOfWork]
        private void executeSubmit(WorkFlowExecute executeModel, bool isSubFlow = false, User doUser = null)
        {
            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            //{
            //如果是第一步提交并且没有实例则先创建实例
            var currentUser = new User();
            if (doUser == null)
                currentUser = _useRepository.Get(AbpSession.UserId.Value);
            else
                currentUser = doUser;
            WorkFlowTask currentTask = null;
            bool isFirst = executeModel.StepID == wfInstalled.FirstStepID && executeModel.TaskID == Guid.Empty && executeModel.GroupID == Guid.Empty;
            if (isFirst)
            {
                currentTask = createFirstTask(executeModel);
                executeModel.TaskID = currentTask.Id;
            }
            else
            {
                currentTask = _workFlowTaskRepository.Get(executeModel.TaskID);
                if (currentTask == null)
                {
                    throw new Exception("未找到要提交的任务");
                }
                //使用程序创建的第一步任务有时候instanceID没有值，需要在这里更新一下
                if (currentTask.InstanceID.IsNullOrEmpty() && !executeModel.InstanceID.IsNullOrEmpty())
                {
                    currentTask.InstanceID = executeModel.InstanceID;
                    _workFlowTaskRepository.Update(currentTask);
                }
            }


            //对自动结束步骤的验证， 若是自动结束步骤， 只能有它， 不能有其它步骤
            var nextStepIds = executeModel.Steps.Select(r => r.Key);
            var nextStepsModel = wfInstalled.Steps.Where(p => nextStepIds.Contains(p.ID));
            if (nextStepsModel.Any(r => r.IsAutoCompleteStep) && nextStepIds.Count() > 1)
            {
                result.DebugMessages = "自动结束步骤只能作为唯一处理步骤";
                result.IsSuccess = false;
                result.Messages = "自动结束步骤只能作为唯一处理步骤";
                return;
            }


            if (currentTask == null)
            {
                result.DebugMessages = "未能创建或找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未能创建或找到当前任务";
                return;
            }
            else if (currentTask.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            else if (currentTask.Status == 10)
            {
                result.DebugMessages = "该流程已被管理员作废";
                result.IsSuccess = false;
                result.Messages = "该流程已被管理员作废";
                return;
            }
            else if (currentTask.ReceiveID != currentUser.Id)
            {
                if (!isSubFlow)
                {
                    var currentStepModel = wfInstalled.Steps.SingleOrDefault(r => r.ID == currentTask.StepID);
                    if (!currentStepModel.IsAutoCompleteStep)
                    {
                        result.DebugMessages = "不能处理当前任务";
                        result.IsSuccess = false;
                        result.Messages = "不能处理当前任务";
                        return;
                    }
                }
            }




            var currentSteps = wfInstalled.Steps.Where(p => p.ID == executeModel.StepID);
            var currentStep = currentSteps.Any() ? currentSteps.First() : null;
            if (currentStep == null)
            {
                result.DebugMessages = "未找到当前步骤";
                result.IsSuccess = false;
                result.Messages = "未找到当前步骤";
                return;
            }
            //查找上一步的完成情况
            Guid prestepid = currentTask.PrevStepID;
            var pretasklist = GetTaskList1(executeModel.FlowID, prestepid, executeModel.GroupID).Where(ite => ite.Type != 5);//排除抄送
            if (pretasklist != null && pretasklist.Any())
            {
                foreach (var p in pretasklist.OrderByDescending(ite => ite.Sort))
                {
                    if (p.Status < 2)
                    {
                        result.DebugMessages = "上一步还未全部提交。";
                        result.IsSuccess = false;
                        result.Messages = "上一步还未全部提交。";
                        return;
                    }
                }
            }


            //如果当前步骤是子流程步骤，并且策略是 子流程完成后才能提交 则要判断子流程是否已完成
            if (currentStep.Type == "subflow"
                && currentStep.SubFlowID.IsGuid()
                && currentStep.Behavior.SubFlowStrategy == 0
                && !currentTask.SubFlowGroupID.IsNullOrEmpty()
                )
            {
                foreach (string groupID in currentTask.SubFlowGroupID.Split(','))
                {
                    if (!GetInstanceIsCompletedWithOutCopyTask(currentStep.SubFlowID.ToGuid(), groupID.ToGuid()))
                    {
                        result.DebugMessages = "当前步骤的子流程实例未完成,子流程：" + currentStep.SubFlowID + ",实例组：" + currentTask.SubFlowGroupID.ToString();
                        result.IsSuccess = false;
                        result.Messages = "当前步骤的子流程未完成,不能提交!";
                        return;
                    }
                }
            }

            int status = 0;//步骤是否通过，为-1  //

            //是否是完成任务或者没有后续处理步骤
            bool isCompletedTask = executeModel.ExecuteType == WorkFlowEnumType.WorkFlowExecuteType.Completed
                || executeModel.Steps == null || executeModel.Steps.Count == 0;

            #region 处理策略判断
            var tjTaskList = GetTaskList1(currentTask.FlowID, currentTask.StepID, currentTask.GroupID);
            if (currentTask.StepID != wfInstalled.FirstStepID)//第一步不判断策略
            {
                switch (currentStep.Behavior.HanlderModel)
                {
                    case 0://所有人必须处理
                        var taskList = tjTaskList.FindAll(p => p.Sort == currentTask.Sort && p.Type != 5 && p.Type != 7);
                        if (taskList.Count > 1)
                        {
                            var noCompleted = taskList.Where(p => p.Status != 2);
                            if (noCompleted.Count() - 1 > 0)
                            {
                                status = -1;
                            }
                        }
                        if (!isCompletedTask)
                        {
                            Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                        }
                        break;
                    case 1://一人同意即可
                        var taskList1 = tjTaskList.FindAll(p => p.Sort == currentTask.Sort && p.Type != 5 && p.Type != 7);
                        foreach (var task in taskList1)
                        {
                            if (task.Id != currentTask.Id)
                            {
                                if (task.Status.In(-1, 0, 1))
                                {
                                    Completed(task.Id, "", false, 4);
                                }
                            }
                            else
                            {
                                if (!isCompletedTask)
                                {
                                    Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                                }
                            }
                        }
                        break;
                    case 2://依据人数比例
                        var taskList2 = tjTaskList.FindAll(p => p.Sort == currentTask.Sort && p.Type != 5 && p.Type != 7);
                        if (taskList2.Count > 1)
                        {
                            currentStep.Behavior.Percentage = currentStep.Behavior.Percentage ?? 0;
                            decimal percentage = currentStep.Behavior.Percentage.Value <= 0 ? 100 : currentStep.Behavior.Percentage.Value;//比例
                            if ((((decimal)(taskList2.Count(p => p.Status == 2) + 1) / (decimal)taskList2.Count) * 100).Round() < percentage)
                            {
                                status = -1;
                            }
                            else
                            {
                                foreach (var task in taskList2)
                                {
                                    if (task.Id != currentTask.Id && task.Status.In(0, 1))
                                    {
                                        Completed(task.Id, "", false, 4);
                                    }
                                }
                            }
                        }
                        if (!isCompletedTask)
                        {
                            Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                        }
                        break;
                    case 3://独立处理
                        if (!isCompletedTask)
                        {
                            Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                        }
                        break;
                }
            }
            else
            {
                if (!isCompletedTask)
                {
                    Completed(currentTask.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                }
            }
            #endregion

            //如果是完成任务或者没有后续处理步骤，则完成任务
            if (isCompletedTask)
            {
                executeComplete(executeModel);
                #region 如果该任务是子流程任务则要判断是否应该提交主流程步骤
                var subTask = GetTaskList3(Guid.Empty, currentTask.GroupID).Find(p => p.OtherType == 4);
                if (subTask != null)
                {
                    var mainTasks = GetBySubFlowGroupID(subTask.GroupID);
                    bool subFlowIsCompleted = true;
                    foreach (var mainTask in mainTasks)
                    {
                        if (!subFlowIsCompleted)
                        {
                            break;
                        }
                        foreach (string subFlowGroupID in mainTask.SubFlowGroupID.Split(','))
                        {
                            if (!GetInstanceIsCompletedWithOutCopyTask(subTask.FlowID, subFlowGroupID.ToGuid()))
                            {
                                subFlowIsCompleted = false;
                                break;
                            }
                        }
                    }
                    if (subFlowIsCompleted)
                    {
                        foreach (var mainTask in mainTasks.OrderByDescending(x=>x.ReceiveID==AbpSession.UserId))
                        {
                            if (mainTask.Status > 2)
                                continue;
                            var autiSubmitResult = AutoSubmit(mainTask, true);
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
                //scope.Complete();

                #region 判断加签
                var noAddWriteTasksWithComplete = tjTaskList.FindAll(p => p.Sort == currentTask.Sort && p.Type != 5 && p.Type != 7);
                foreach (var noAddTask in noAddWriteTasksWithComplete)
                {
                    var addTasks = tjTaskList.FindAll(p => p.PrevID == noAddTask.Id && p.Type == 7);
                    if (noAddTask.Id == currentTask.Id && addTasks.Count > 0)
                    {
                        foreach (var addTask in addTasks)
                        {
                            if (!addTask.OtherType.HasValue)
                            {
                                continue;
                            }
                            int addType = addTask.OtherType.Value.ToString().Left(1).ToString().ToInt();
                            int writeType = addTask.OtherType.Value.ToString().Right(1).ToString().ToInt();
                            //如果是后加签本步骤通过了则要将后加签的人的待办状态更新为0
                            if (addType == 2)
                            {
                                if (writeType.In(1, 2) || (writeType == 3 && addTask.ReceiveID == addTasks.FindAll(p => p.Status.In(-1, 0, 1)).OrderBy(p => p.ReceiveTime).FirstOrDefault().ReceiveID))
                                {
                                    addTask.Status = 0;
                                    _workFlowTaskRepository.Update(addTask);
                                }
                            }
                        }
                    }
                    var addWriteTasks1 = new List<WorkFlowTask>();
                    if (addTasks.Count > 0 && !isPassingAddWrite(addTasks.FirstOrDefault(), out addWriteTasks1))
                    {
                        status = -2;
                        break;
                    }
                }
                #endregion

                return;
            }

            #region 判断加签
            var noAddWriteTasks = tjTaskList.FindAll(p => p.Sort == currentTask.Sort && p.Type != 5 && p.Type != 7);
            foreach (var noAddTask in noAddWriteTasks)
            {
                var addTasks = tjTaskList.FindAll(p => p.PrevID == noAddTask.Id && p.Type == 7);
                if (noAddTask.Id == currentTask.Id && addTasks.Count > 0)
                {
                    foreach (var addTask in addTasks)
                    {
                        if (!addTask.OtherType.HasValue)
                        {
                            continue;
                        }
                        int addType = addTask.OtherType.Value.ToString().Left(1).ToString().ToInt();
                        int writeType = addTask.OtherType.Value.ToString().Right(1).ToString().ToInt();
                        //如果是后加签本步骤通过了则要将后加签的人的待办状态更新为0
                        if (addType == 2)
                        {
                            if (writeType.In(1, 2) || (writeType == 3 && addTask.ReceiveID == addTasks.FindAll(p => p.Status.In(-1, 0, 1)).OrderBy(p => p.ReceiveTime).FirstOrDefault().ReceiveID))
                            {
                                addTask.Status = 0;
                                _workFlowTaskRepository.Update(addTask);
                            }
                        }
                    }
                }
                var addWriteTasks1 = new List<WorkFlowTask>();
                if (addTasks.Count > 0 && !isPassingAddWrite(addTasks.FirstOrDefault(), out addWriteTasks1))
                {
                    status = -2;
                    break;
                }
            }
            #endregion

            //如果条件不满足则创建一个状态为-1的后续任务，等条件满足后才修改状态，待办人员才能看到任务。
            if (status == -1 || status == -2)
            {
                var tempTasks = createTempTasks(executeModel, currentTask);
                List<string> nextStepName = new List<string>();
                foreach (var nstep in tempTasks)
                {
                    nextStepName.Add(nstep.StepName);
                }
                nextTasks.AddRange(tempTasks);
                string stepName = nextStepName.Distinct().ToArray().Join1(",");
                result.DebugMessages += string.Format("已发送到:{0},{1},不创建后续任务", stepName, status == -2 ? "加签人未处理" : "其他人未处理");
                result.IsSuccess = true;
                result.Messages += string.Format("已发送到:{0},{1}!", stepName, status == -2 ? "等待加签人处理" : "等待他人处理");
                result.NextTasks = nextTasks;
                //scope.Complete();
                return;
            }


            var autoSubmitTasks = new List<WorkFlowTask>();//记录需要自动提交的子流程任务



            foreach (var step in executeModel.Steps)
            {


                var subflowGroupOneID = Guid.NewGuid();//如果子流程是多人同一实例，所有任务使用一个组ID
                var hasCreateSubflowInstance = false;
                var firestCreateSubflowInstanceId = "";
                StringBuilder subflowGroupID = new StringBuilder();//如果子流程是多人单独实例，保存每个任务的组ID
                var tempTasks = new List<WorkFlowTask>();



                foreach (var user in step.Value)
                {
                    if (wfInstalled == null) //子流程有多个人员时此处会为空，所以判断并重新获取
                    {
                        wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(executeModel.FlowID, executeModel.VersionNum);

                    }
                    var nextSteps = wfInstalled.Steps.Where(p => p.ID == step.Key);
                    if (!nextSteps.Any())
                    {
                        continue;
                    }
                    var nextStep = nextSteps.First();


                    bool isPassing = 0 == nextStep.Behavior.Countersignature;

                    #region 如果下一步骤为会签，则要检查当前步骤的平级步骤是否已处理
                    if (0 != nextStep.Behavior.Countersignature)
                    {
                        var prevSteps = GetPrevSteps(executeModel.FlowID, nextStep.ID);
                        if (prevSteps.Count > 1)
                        {
                            Guid currentPrevStep = currentTask.PrevStepID;
                            switch (nextStep.Behavior.Countersignature)
                            {
                                case 1://所有步骤同意
                                    isPassing = true;
                                    foreach (var prevStep in prevSteps)
                                    {
                                        if (!IsPassing(prevStep, executeModel.FlowID, executeModel.GroupID, currentTask, currentPrevStep))
                                        {
                                            isPassing = false;
                                            break;
                                        }
                                    }
                                    break;
                                case 2://一个步骤同意即可
                                    isPassing = false;
                                    foreach (var prevStep in prevSteps)
                                    {
                                        if (IsPassing(prevStep, executeModel.FlowID, executeModel.GroupID, currentTask, currentPrevStep))
                                        {
                                            isPassing = true;
                                            break;
                                        }
                                    }
                                    break;
                                case 3://依据比例
                                    int passCount = 0;
                                    if (prevSteps.Count == 0)
                                    {
                                        isPassing = true;
                                    }
                                    else
                                    {
                                        foreach (var prevStep in prevSteps)
                                        {
                                            if (IsPassing(prevStep, executeModel.FlowID, executeModel.GroupID, currentTask, currentPrevStep))
                                            {
                                                passCount++;
                                            }
                                        }
                                        isPassing = (((decimal)passCount / (decimal)prevSteps.Count) * 100).Round() >= (nextStep.Behavior.CountersignaturePercentage <= 0 ? 100 : nextStep.Behavior.CountersignaturePercentage);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            isPassing = true;
                        }
                        if (isPassing)
                        {
                            var tjTasks = GetTaskList2(currentTask.Id, false);
                            foreach (var tjTask in tjTasks)
                            {
                                if (tjTask.Id == currentTask.Id || tjTask.Type == 5 || tjTask.Status.In(2, 3, 4, 5, 6, 7))
                                {
                                    continue;
                                }
                                Completed(tjTask.Id, "", false, 4);
                            }
                        }
                    }
                    #endregion

                    if (isPassing)
                    {
                        var task = new WorkFlowTask();
                        if (nextStep.WorkTime > 0)
                        {
                            task.CompletedTime = DateTime.Now.AddHours((double)nextStep.WorkTime);
                        }

                        task.FlowID = executeModel.FlowID;
                        task.GroupID = currentTask != null ? currentTask.GroupID : executeModel.GroupID;
                        task.Id = Guid.NewGuid();
                        task.Type = 0;
                        task.InstanceID = executeModel.InstanceID;
                        if (!executeModel.Note.IsNullOrEmpty())
                        {
                            task.Note = executeModel.Note;
                        }
                        task.PrevID = currentTask.Id;
                        task.PrevStepID = currentTask.StepID;
                        task.ReceiveID = user.Id;
                        task.ReceiveName = user.Name;
                        task.ReceiveTime = DateTime.Now;
                        task.SenderID = executeModel.Sender.Id;
                        task.SenderName = executeModel.Sender.Name;
                        task.SenderTime = task.ReceiveTime;
                        task.Status = status;
                        task.StepID = step.Key;
                        if (executeModel.Users != null && executeModel.Users.Count > 0)
                        {
                            var taskUser = executeModel.Users.FirstOrDefault(x => x.NextStepId == step.Key && x.RelationUserId == user.Id);
                            if (taskUser != null)
                                task.RelationId = taskUser.RelationId;
                        }
                        task.StepName = nextStep.Name;
                        task.Sort = currentTask.Sort + 1;
                        task.Title = executeModel.Title.IsNullOrEmpty() ? currentTask.Title : executeModel.Title;
                        task.OtherType = executeModel.OtherType;
                        task.VersionNum = executeModel.VersionNum;
                        task.Deepth = currentTask.Deepth;

                        #region 如果当前步骤是子流程步骤，则要发起子流程实例
                        if (nextStep.Type == "subflow" && nextStep.SubFlowID.IsGuid())
                        {
                            var subFlowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(nextStep.SubFlowID.ToGuid());
                            List<WorkFlowExecute> subflowExecuteModelList = new List<WorkFlowExecute>();


                            if (nextStep.SubFlowTaskType == 0)
                            {
                                var subflowExecuteModel = new WorkFlowExecute();

                                if (!hasCreateSubflowInstance)
                                {
                                    object obj = ExecuteFlowCustomEvent(nextStep.Event.SubFlowActivationBefore.Trim(),
                                       new WorkFlowCustomEventParams()
                                       {
                                           FlowID = executeModel.FlowID,
                                           GroupID = currentTask.GroupID,
                                           InstanceID = currentTask.InstanceID,
                                           StepID = executeModel.StepID,
                                           TaskID = currentTask.Id,
                                           NextRecevieUserId = user.Id,
                                       });

                                    if (obj is WorkFlowExecute)
                                    {
                                        subflowExecuteModel = obj as WorkFlowExecute;
                                        hasCreateSubflowInstance = true;
                                        firestCreateSubflowInstanceId = subflowExecuteModel.InstanceID;
                                    }
                                    else
                                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "子流程实例激活事件失败");
                                }
                                else
                                {
                                    subflowExecuteModel = new WorkFlowExecute();
                                    subflowExecuteModel.InstanceID = firestCreateSubflowInstanceId;
                                }

                                subflowExecuteModel.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Save;
                                subflowExecuteModel.FlowID = nextStep.SubFlowID.ToGuid();
                                subflowExecuteModel.VersionNum = subFlowModel.VersionNum;
                                subflowExecuteModel.Sender = user;
                                if (subflowExecuteModel.Title.IsNullOrEmpty())
                                {
                                    subflowExecuteModel.Title = GetFlowName(subflowExecuteModel.FlowID);
                                }
                                if (subflowExecuteModel.InstanceID.IsNullOrEmpty())
                                {
                                    subflowExecuteModel.InstanceID = "";
                                }
                                subflowExecuteModel.GroupID = subflowGroupOneID;
                                subflowExecuteModel.OtherType = 4;
                                var subflowTask = new WorkFlowTask();
                                if (nextStep.IsSubFlowTaskMerge)
                                {
                                    subflowTask = createSubFlowFirstTask(subflowExecuteModel);
                                    subflowGroupOneID = subflowTask.GroupID;
                                }
                                else
                                    subflowTask = createFirstTask(subflowExecuteModel, true);

                                task.ReceiveID = user.Id;
                                task.ReceiveName = user.Name;
                                //将SubFlowGroupID标识为空GUID，以便后面判断任务有子流程，设置SubFlowGroupID
                                task.SubFlowGroupID = Guid.Empty.ToString();
                                //将子流程处理策略临时保存到这个字段，以便后面判断，判断之后改回空
                                task.OtherType = nextStep.Behavior.SubFlowStrategy == 0 ? 2 : 3;
                                //将当前任务类型标识为子流程任务
                                task.Type = 6;
                                task.Title = subFlowModel.Name;
                                nextTasks.Add(task);
                                tempTasks.Add(task);
                            }
                            else
                            {
                                object obj = ExecuteFlowCustomEvent(nextStep.Event.SubFlowActivationBefore.Trim(),
                                      new WorkFlowCustomEventParams()
                                      {
                                          FlowID = executeModel.FlowID,
                                          GroupID = currentTask.GroupID,
                                          InstanceID = currentTask.InstanceID,
                                          StepID = executeModel.StepID,
                                          TaskID = currentTask.Id,
                                          NextRecevieUserId = user.Id,
                                      });
                                if (obj is List<WorkFlowExecute>)
                                {
                                    subflowExecuteModelList = obj as List<WorkFlowExecute>;
                                }

                                if (obj is WorkFlowExecute)
                                {
                                    var ret = obj as WorkFlowExecute;
                                    subflowExecuteModelList = new List<WorkFlowExecute>() { ret };
                                }


                                foreach (var item in subflowExecuteModelList)
                                {

                                    item.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Save;
                                    item.FlowID = nextStep.SubFlowID.ToGuid();
                                    item.VersionNum = subFlowModel.VersionNum;
                                    item.Sender = user;
                                    if (item.Title.IsNullOrEmpty())
                                    {
                                        item.Title = GetFlowName(item.FlowID);
                                    }
                                    if (item.InstanceID.IsNullOrEmpty())
                                    {
                                        item.InstanceID = "";
                                    }
                                    item.GroupID = Guid.NewGuid();
                                    subflowGroupID.Append(item.GroupID.ToString());
                                    subflowGroupID.Append(",");
                                    item.OtherType = 4;
                                    var subflowTask = new WorkFlowTask();
                                    subflowTask = createFirstTask(item, true);
                                    //如果是子流程则任务接收人根据步骤策略去找；
                                    var taksEntity = task.DeepClone();
                                    taksEntity.Id = Guid.NewGuid();
                                    taksEntity.ReceiveID = user.Id;
                                    taksEntity.ReceiveName = user.Name;
                                    //将SubFlowGroupID标识为空GUID，以便后面判断任务有子流程，设置SubFlowGroupID
                                    taksEntity.SubFlowGroupID = Guid.Empty.ToString();
                                    //将子流程处理策略临时保存到这个字段，以便后面判断，判断之后改回空
                                    taksEntity.OtherType = nextStep.Behavior.SubFlowStrategy == 0 ? 2 : 3;
                                    //将当前任务类型标识为子流程任务
                                    taksEntity.Type = 6;
                                    taksEntity.Title = subFlowModel.Name;
                                    nextTasks.Add(taksEntity);
                                    tempTasks.Add(taksEntity);

                                }
                            }




                        }
                        else
                        {
                            nextTasks.Add(task);
                            tempTasks.Add(task);
                        }



                        #endregion
                    }


                }

                foreach (var nextTask in tempTasks)
                {
                    if (!HasNoCompletedTasks(executeModel.FlowID, step.Key, currentTask.GroupID, nextTask.ReceiveID))
                    {
                        var currentFlowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(executeModel.FlowID, executeModel.VersionNum);
                        if (nextTask.Type == 6)//如果是子流程任务则要更新SubFlowGroupID值，关联子流程的GroupID
                        {
                            if (subflowGroupID.Length == 0)
                            {
                                nextTask.SubFlowGroupID = subflowGroupOneID.ToString();
                            }
                            else
                            {
                                nextTask.SubFlowGroupID = subflowGroupID.ToString().TrimEnd(',');
                            }
                            //nextTask.OtherType = null;
                            var nextTaskStepModel = currentFlowModel.Steps.FirstOrDefault(r => r.ID == nextTask.StepID);
                            if (nextTaskStepModel.IsHideTask)
                                nextTask.Status = -1;
                            _workFlowTaskRepository.Insert(nextTask);
                            if (nextTask.OtherType == 3)
                            {
                                autoSubmitTasks.Add(nextTask);
                            }
                        }
                        else
                        {
                            var nextTaskStepModel = currentFlowModel.Steps.FirstOrDefault(r => r.ID == nextTask.StepID);
                            if (nextTaskStepModel.IsHideTask)
                                nextTask.Status = -1;
                            _workFlowTaskRepository.Insert(nextTask);

                            #region  步骤发送后（新的待办创建后），维护业务表的DealWithUsers字段
                            UpdateInstanceDealWithUsers(currentTask, nextTask);
                            #endregion

                        }
                    }
                    else
                    {
                        var currentNextTask = nextTasks.FirstOrDefault(r => r.Id == nextTask.Id);
                        var exit_StepUserTask = _workFlowTaskRepository.FirstOrDefault(r => r.FlowID == executeModel.FlowID && r.StepID == step.Key
                        && r.GroupID == currentTask.GroupID && r.ReceiveID == nextTask.ReceiveID);
                        if (exit_StepUserTask == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
                        currentNextTask.Id = exit_StepUserTask.Id;
                    }
                }
            }

            if (nextTasks.Count > 0)
            {
                //激活临时任务
                var nextWaitSteps = nextTasks.GroupBy(p => p.StepID);
                foreach (var nextStep in nextWaitSteps)
                {
                    UpdateTempTasks(nextTasks.FirstOrDefault().FlowID, nextStep.Key, nextTasks.FirstOrDefault().GroupID,
                        nextTasks.FirstOrDefault().CompletedTime, nextTasks.FirstOrDefault().ReceiveTime);
                }



                //将当前步骤未处理的任务完成   --zcl 为独立处理时， 跳过这个逻辑
                if (currentStep.Behavior.HanlderModel != 3)
                {
                    if (executeModel.OtherType != 1) //如果是自由流程任务，则不完成
                    {
                        var currStepTasks = GetTaskList1(currentTask.FlowID, currentTask.StepID, currentTask.GroupID);
                        foreach (var currStepTask in currStepTasks)
                        {
                            if (currStepTask.Type == 5)
                                continue;
                            if (currStepTask.Status.In(-1, 0, 1) && currStepTask.OtherType != 1)
                            {
                                Completed(currStepTask.Id, "", false, 4);
                            }
                        }
                    }
                }


                #region 抄送
                if (wfInstalled == null)
                {
                    wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(executeModel.FlowID, executeModel.VersionNum);
                }
                foreach (var step in executeModel.Steps)
                {
                    var nextSteps = wfInstalled.Steps.Where(p => p.ID == step.Key);
                    if (nextSteps.Any())
                    {
                        var nextStep = nextSteps.First();
                        if (nextStep.Behavior.IsCopyFor)
                        {
                            WrokFlowTaskCopyFor(currentTask, nextStep, executeModel, step.Key, 0, 1);
                        }
                    }
                }
                #endregion

                List<string> nextStepName = new List<string>();
                foreach (var nstep in nextTasks)
                {
                    nextStepName.Add(nstep.StepName);
                }
                string stepName = nextStepName.Distinct().ToArray().Join1(",");
                result.DebugMessages += $"已发送到:{stepName}";
                result.IsSuccess = true;
                result.Messages += $"已发送到:{stepName}";
                result.NextTasks = nextTasks;
                if (nextTasks.Count() == 1)
                {
                    var nextTask = nextTasks.FirstOrDefault();
                    var nextTaskStepModel = wfInstalled.Steps.FirstOrDefault(r => r.ID == nextTask.StepID);
                    if (nextTaskStepModel.IsAutoCompleteStep)
                    {
                        _workFlowTaskRepository.Insert(nextTask);
                        CurrentUnitOfWork.SaveChanges();
                        result.Messages = "已完成";
                    }
                }
            }
            else
            {
                var tempTasks = createTempTasks(executeModel, currentTask);
                List<string> nextStepName = new List<string>();
                foreach (var nstep in tempTasks)
                {
                    nextStepName.Add(nstep.StepName);
                }
                nextTasks.AddRange(tempTasks);
                string stepName = nextStepName.Distinct().ToArray().Join1(",");
                result.DebugMessages += $"已发到:{stepName},等待其它步骤处理";
                result.IsSuccess = true;
                result.Messages += $"已发送:{stepName},等待其它步骤处理";
                result.NextTasks = nextTasks;
            }

            //如果子流程发起即提交，则要立即提交任务
            if (autoSubmitTasks.Count > 0)
            {
                foreach (var subTask in autoSubmitTasks)
                {
                    AutoSubmit(subTask, true);
                }
            }



        }

        [UnitOfWork]
        private void executeCopyforComplete(WorkFlowExecute executeModel)
        {
            if (executeModel.TaskID == Guid.Empty || executeModel.FlowID == Guid.Empty)
            {
                result.DebugMessages = "完成流程参数错误";
                result.IsSuccess = false;
                result.Messages = "完成流程参数错误";
                return;
            }
            var task = _workFlowTaskRepository.Get(executeModel.TaskID);
            if (task == null)
            {
                result.DebugMessages = "未找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未找到当前任务";
                return;
            }
            else if (task.Type != 5)
            {
                result.DebugMessages = "当前不是抄送任务";
                result.IsSuccess = false;
                result.Messages = "当前不是抄送任务";
                return;
            }
            else if (task.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
            result.DebugMessages += "已完成";
            result.IsSuccess = true;
            result.Messages += "已完成";
        }

        [UnitOfWork]
        private void executeInquiryComplete(WorkFlowExecute executeModel)
        {
            if (executeModel.TaskID == Guid.Empty || executeModel.FlowID == Guid.Empty)
            {
                result.DebugMessages = "完成流程参数错误";
                result.IsSuccess = false;
                result.Messages = "完成流程参数错误";
                return;
            }
            var task = _workFlowTaskRepository.Get(executeModel.TaskID);
            if (task == null)
            {
                result.DebugMessages = "未找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未找到当前任务";
                return;
            }
            else if (task.Type != 8)
            {
                result.DebugMessages = "当前不是意见征询任务";
                result.IsSuccess = false;
                result.Messages = "当前不是意见征询任务";
                return;
            }
            else if (task.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
            result.DebugMessages += "已完成";
            result.IsSuccess = true;
            result.Messages += "已完成";
        }


        public void WrokFlowTaskCopyFor(WorkFlowTask currentTask, WorkFlowStep nextStep, WorkFlowExecute executeModel, Guid stepID, int status, int type)
        {
            string selectType, selectRange;
            var defultMembers = GetDefultMemberByCopyFor(type, currentTask.FlowID, nextStep.ID, currentTask.GroupID, currentTask.StepID, currentTask.InstanceID, out selectType, out selectRange, currentTask.Id, currentTask.VersionNum);
            if (string.IsNullOrEmpty(defultMembers))
                return;
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = organizeManager.GetAllUsers(defultMembers);
            foreach (var user in users)
            {
                var task = new WorkFlowTask();
                if (nextStep.WorkTime > 0)
                {
                    task.CompletedTime = DateTime.Now.AddHours((double)nextStep.WorkTime);
                }
                task.FlowID = executeModel.FlowID;
                task.GroupID = currentTask != null ? currentTask.GroupID : executeModel.GroupID;
                task.Id = Guid.NewGuid();
                task.Type = 5;
                task.InstanceID = executeModel.InstanceID;
                task.Note = executeModel.Note.IsNullOrEmpty() ? "抄送任务" : executeModel.Note + "(抄送任务)";
                task.PrevID = currentTask.Id;
                task.PrevStepID = currentTask.StepID;
                task.ReceiveID = user.Id;
                task.ReceiveName = user.Name;
                task.ReceiveTime = DateTime.Now;
                task.SenderID = executeModel.Sender.Id;
                task.SenderName = executeModel.Sender.Name;
                task.SenderTime = task.ReceiveTime;
                task.Status = status;
                task.StepID = stepID;
                task.StepName = "抄送";
                task.Sort = currentTask.Sort;
                task.TodoType = currentTask.TodoType;
                task.Deepth = currentTask.Deepth;
                task.VersionNum = currentTask.VersionNum;
                task.Title = executeModel.Title.IsNullOrEmpty() ? currentTask.Title : executeModel.Title;
                if (!HasNoCompletedTasks(executeModel.FlowID, stepID, currentTask.GroupID, user.Id))
                {
                    _workFlowTaskRepository.Insert(task);
                }
            }
        }


        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="executeModel"></param>
        /// <param name="isCompleteTask">是否需要调用Completed方法完成当前任务</param>
        [UnitOfWork]
        private void executeComplete(WorkFlowExecute executeModel, bool isCompleteTask = true)
        {
            if (executeModel.TaskID == Guid.Empty || executeModel.FlowID == Guid.Empty)
            {
                result.DebugMessages = "完成流程参数错误";
                result.IsSuccess = false;
                result.Messages = "完成流程参数错误";
                return;
            }
            var task = _workFlowTaskRepository.Get(executeModel.TaskID);
            if (task == null)
            {
                result.DebugMessages = "未找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未找到当前任务";
                return;
            }
            else if (isCompleteTask && task.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            if (isCompleteTask)
            {
                Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
            }
            var currentwfInstallModel = _workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID);

            #region 更新业务表标识字段的值为1
            if (currentwfInstallModel.TitleField != null && !currentwfInstallModel.TitleField.Table.IsNullOrEmpty()
                && !currentwfInstallModel.TitleField.Field.IsNullOrEmpty() && currentwfInstallModel.DataBases.Any())
            {
                var firstDB = currentwfInstallModel.DataBases.First();
                try
                {
                    string sql = $"UPDATE {currentwfInstallModel.TitleField.Table} SET {currentwfInstallModel.TitleField.Field}=\'-1\' WHERE {firstDB.PrimaryKey}=\'{task.InstanceID}\'";
                    _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                    var taskManagementRelation = _taskManagementRelation.GetAll().FirstOrDefault(x => x.FlowId == task.FlowID && x.InStanceId == task.InstanceID.ToGuid());
                    if (taskManagementRelation != null)
                    {
                        _eventBus.Trigger(new TaskManagementData() { Id = taskManagementRelation.TaskManagementId, TaskStatus = TaskManagementStateEnum.Done });
                    }
                }
                catch (Exception err)
                {

                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                }
            }
            #endregion

            #region 执行子流程完成后事件
            var parentTasks = GetBySubFlowGroupID(task.GroupID);
            if (parentTasks.Count > 0)
            {
                var parentTask = parentTasks.First();
                var flowRunModel = _workFlowCacheManager.GetWorkFlowModelFromCache(parentTask.FlowID, parentTask.VersionNum);
                if (flowRunModel != null)
                {
                    var steps = flowRunModel.Steps.Where(p => p.ID == parentTask.StepID);
                    if (steps.Any() && !steps.First().Event.SubFlowCompletedBefore.IsNullOrEmpty())
                    {
                        ExecuteFlowCustomEvent(steps.First().Event.SubFlowCompletedBefore.Trim(), new WorkFlowCustomEventParams()
                        {
                            FlowID = parentTask.FlowID,
                            GroupID = parentTask.GroupID,
                            InstanceID = parentTask.InstanceID,
                            StepID = parentTask.StepID,
                            TaskID = parentTask.Id
                        });
                    }
                }
            }
            #endregion

            result.DebugMessages += "已完成";
            result.IsSuccess = true;
            result.Messages += "已完成";
        }


        /// <summary>
        /// 完成一个任务
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="comment"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        public int Completed(Guid taskID, string comment = "", bool isSign = false, int status = 2, string note = "", string files = "")
        {
            try
            {
                var taskmodel = _workFlowTaskRepository.Get(taskID);
                taskmodel.Comment = comment;
                taskmodel.CompletedTime1 = DateTime.Now;
                taskmodel.IsSign = isSign ? 1 : 0;
                taskmodel.Status = status;
                //签名
                if (!taskmodel.SignFileId.HasValue)
                {
                    var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(taskmodel.FlowID, taskmodel.VersionNum);
                    var stepModel = flow.Steps.FirstOrDefault(x => x.ID == taskmodel.StepID);
                    if (stepModel != null && stepModel.SignatureType == 2)
                    {
                        var signFileId = _employeesSignManager.GetSignFileId(taskmodel.ReceiveID);
                        if (signFileId.HasValue)
                            taskmodel.SignFileId = signFileId;
                    }
                }
                if (!note.IsNullOrEmpty())
                    taskmodel.Note = note;
                taskmodel.Files = files.IsNullOrEmpty() ? null : files;
                _workFlowTaskRepository.Update(taskmodel);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }


        }


        /// <summary>
        /// 创建第一个任务
        /// </summary>
        /// <param name="executeModel"></param>
        /// <param name="isSubFlow">是否是创建子流程任务</param>
        /// <returns></returns>
        private WorkFlowTask createFirstTask(WorkFlowExecute executeModel, bool isSubFlow = false)
        {
            if (wfInstalled == null || isSubFlow)
            {
                wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(executeModel.FlowID, executeModel.VersionNum);
            }

            var nextSteps = wfInstalled.Steps.Where(p => p.ID == wfInstalled.FirstStepID);
            if (!nextSteps.Any())
            {
                return null;
            }
            var task = new WorkFlowTask();
            if (nextSteps.First().WorkTime > 0)
            {
                task.CompletedTime = DateTime.Now.AddHours((double)nextSteps.First().WorkTime);
            }
            task.FlowID = executeModel.FlowID;
            task.GroupID = executeModel.GroupID.IsEmptyGuid() ? Guid.NewGuid() : executeModel.GroupID;
            task.Id = Guid.NewGuid();
            task.Type = 0;
            task.InstanceID = executeModel.InstanceID;
            if (!executeModel.Note.IsNullOrEmpty())
            {
                task.Note = executeModel.Note;
            }
            task.PrevID = Guid.Empty;
            task.PrevStepID = Guid.Empty;
            task.ReceiveID = executeModel.Sender.Id;
            task.ReceiveName = executeModel.Sender.Name;
            task.ReceiveTime = DateTime.Now;
            task.SenderID = executeModel.Sender.Id;
            task.SenderName = executeModel.Sender.Name;
            task.SenderTime = task.ReceiveTime;
            task.Status = 0;
            task.StepID = wfInstalled.FirstStepID;
            task.StepName = nextSteps.First().Name;
            task.Sort = 1;
            task.OtherType = executeModel.OtherType;
            task.Deepth = 1;
            task.Title = executeModel.Title.IsNullOrEmpty() ? "未命名任务(" + wfInstalled.Name + ")" : executeModel.Title;
            task.VersionNum = executeModel.VersionNum;
            _workFlowTaskRepository.Insert(task);
            if (isSubFlow)
            {
                wfInstalled = null;
            }
            return task;
        }


        /// <summary>
        /// 创建子流程第一个任务
        /// </summary>
        /// <param name="executeModel"></param>
        /// <param name="isSubFlow">是否是创建子流程任务</param>
        /// <returns></returns>
        private WorkFlowTask createSubFlowFirstTask(WorkFlowExecute executeModel)
        {
            bool isSubFlow = true;
            if (wfInstalled == null || isSubFlow)
            {
                wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(executeModel.FlowID, executeModel.VersionNum);
            }

            var nextSteps = wfInstalled.Steps.Where(p => p.ID == wfInstalled.FirstStepID);
            if (!nextSteps.Any())
            {
                return null;
            }

            if (HasNoCompletedTasksWithInstanceId(executeModel.FlowID, nextSteps.First().ID, executeModel.InstanceID, executeModel.Sender.Id))
            {
                var exitSub_Task = _workFlowTaskRepository.GetAll().FirstOrDefault(r => r.FlowID == executeModel.FlowID && r.StepID == nextSteps.First().ID && r.InstanceID == executeModel.InstanceID && r.ReceiveID == executeModel.Sender.Id);
                return exitSub_Task;
            }

            var task = new WorkFlowTask();
            if (nextSteps.First().WorkTime > 0)
            {
                task.CompletedTime = DateTime.Now.AddHours((double)nextSteps.First().WorkTime);
            }
            task.FlowID = executeModel.FlowID;
            task.GroupID = executeModel.GroupID.IsEmptyGuid() ? Guid.NewGuid() : executeModel.GroupID;
            task.Id = Guid.NewGuid();
            task.Type = 0;
            task.InstanceID = executeModel.InstanceID;
            if (!executeModel.Note.IsNullOrEmpty())
            {
                task.Note = executeModel.Note;
            }
            task.PrevID = Guid.Empty;
            task.PrevStepID = Guid.Empty;
            task.ReceiveID = executeModel.Sender.Id;
            task.ReceiveName = executeModel.Sender.Name;
            task.ReceiveTime = DateTime.Now;
            task.SenderID = executeModel.Sender.Id;
            task.SenderName = executeModel.Sender.Name;
            task.SenderTime = task.ReceiveTime;
            task.Status = 0;
            task.StepID = wfInstalled.FirstStepID;
            task.StepName = nextSteps.First().Name;
            task.Sort = 1;
            task.OtherType = executeModel.OtherType;
            task.Deepth = 1;
            task.Title = executeModel.Title.IsNullOrEmpty() ? "未命名任务(" + wfInstalled.Name + ")" : executeModel.Title;
            task.VersionNum = executeModel.VersionNum;
            _workFlowTaskRepository.Insert(task);
            if (isSubFlow)
            {
                wfInstalled = null;
            }
            return task;
        }


        /// <summary>
        /// 自动提交一个任务
        /// </summary>
        /// <param name="task">任务实体</param>
        /// <returns></returns>
        public WorkFlowResult AutoSubmit(WorkFlowTask task, bool isSubFlow = false)
        {
            var autoSubmitResult = new WorkFlowResult();
            if (task == null)
            {
                autoSubmitResult.DebugMessages = "未找到任务";
                autoSubmitResult.IsSuccess = false;
                autoSubmitResult.Messages = "未找到任务";
                return autoSubmitResult;
            }
            if (!task.Status.In(-1, 0, 1))
            {
                autoSubmitResult.DebugMessages = "任务已完成";
                autoSubmitResult.IsSuccess = false;
                autoSubmitResult.Messages = "任务已完成";
                return autoSubmitResult;
            }
            var nextSteps = GetNextSteps(task.FlowID, task.StepID, task.VersionNum);
            if (nextSteps.Count == 0)
            {
                if (isSubFlow)
                {
                    //executeComplete(new WorkFlowExecute()
                    //{
                    //    ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Completed,
                    //    FlowID = task.FlowID,
                    //    GroupID = task.GroupID,
                    //    InstanceID = task.InstanceID,
                    //    Comment = "",
                    //    IsSign = false,
                    //    TaskID = task.Id,
                    //});
                    //子流程执行完成修改
                    var subExecute = new WorkFlowExecute();
                    subExecute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Completed;
                    subExecute.FlowID = task.FlowID;
                    subExecute.GroupID = task.GroupID;
                    subExecute.InstanceID = task.InstanceID;
                    var currentUser = _useRepository.Get(task.ReceiveID);
                    subExecute.Sender = currentUser;
                    subExecute.StepID = task.StepID;
                    subExecute.TaskID = task.Id;
                    subExecute.Title = task.Title;
                    subExecute.IsSign = false;
                    subExecute.OtherType = 0;
                    subExecute.VersionNum = task.VersionNum;
                    var executeResult = Execute(subExecute, isSubFlow);
                    return executeResult;
                }
                else
                {
                    autoSubmitResult.DebugMessages = "当前步骤没有后续步骤";
                    autoSubmitResult.IsSuccess = false;
                    autoSubmitResult.Messages = "当前步骤没有后续步骤";
                    return autoSubmitResult;
                }
            }
            var sendSteps = new Dictionary<Guid, List<User>>();
            var roadFlowOrganizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            foreach (var nextStep in nextSteps)
            {
                string selectType, selectRange;
                var defaultMemberString = GetDefultMember(task.FlowID, nextStep.ID, task.GroupID, task.StepID, task.InstanceID, out selectType, out selectRange, task.Id, task.VersionNum);
                if (defaultMemberString.IsNullOrEmpty())
                {
                    continue;
                }

                var users = roadFlowOrganizeManager.GetAllUsers(defaultMemberString);
                if (users.Count == 0)
                {
                    continue;
                }
                sendSteps.Add(nextStep.ID, users);
            }
            if (sendSteps.Count > 0)
            {
                var subExecute = new WorkFlowExecute();
                subExecute.ExecuteType = WorkFlowEnumType.WorkFlowExecuteType.Submit;
                subExecute.FlowID = task.FlowID;
                subExecute.GroupID = task.GroupID;
                subExecute.InstanceID = task.InstanceID;
                var currentUser = _useRepository.Get(task.ReceiveID);
                subExecute.Sender = currentUser;
                subExecute.StepID = task.StepID;
                subExecute.Steps = sendSteps;
                subExecute.TaskID = task.Id;
                subExecute.Title = task.Title;
                subExecute.IsSign = false;
                subExecute.OtherType = 0;
                subExecute.VersionNum = task.VersionNum;
                var executeResult = Execute(subExecute, isSubFlow);
                if (executeResult.IsSuccess)
                {
                    var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
                    var mainTaskWorkFlowModel = workFlowCacheManager.GetWorkFlowModelFromCache(subExecute.FlowID, subExecute.VersionNum);
                    #region   主流程步骤提交   改变实例状态 ； 下一步有多个步骤时 状态无意义；

                    foreach (var step in sendSteps)
                    {
                        var stepgid = step.Key;
                        var toStepModel = mainTaskWorkFlowModel.Steps.FirstOrDefault(r => r.ID == stepgid);
                        if (toStepModel == null)
                            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数异常");
                        if (toStepModel.ChangeStatus)
                        {
                            #region 更新业务表标识字段的值为StepToStatus
                            if (mainTaskWorkFlowModel.TitleField != null && mainTaskWorkFlowModel.TitleField.LinkID != Guid.Empty && !mainTaskWorkFlowModel.TitleField.Table.IsNullOrEmpty()
                                && !mainTaskWorkFlowModel.TitleField.Field.IsNullOrEmpty() && mainTaskWorkFlowModel.DataBases.Any())
                            {
                                var firstDB = mainTaskWorkFlowModel.DataBases.First();
                                try
                                {
                                    string sql = $"UPDATE {mainTaskWorkFlowModel.TitleField.Table} SET {mainTaskWorkFlowModel.TitleField.Field}=\'{toStepModel.StepToStatus}\' WHERE {firstDB.PrimaryKey}=\'{subExecute.InstanceID}\'";
                                    _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                                }
                                catch (Exception err)
                                {

                                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                                    Abp.Logging.LogHelper.Logger.Error($"更新流程状态迁移发生了错误({mainTaskWorkFlowModel.Name}),flowid:{mainTaskWorkFlowModel.ID},taskid:{subExecute.TaskID},errormsg:{err.Message}");
                                    throw err;
                                }
                            }
                            #endregion

                        }




                    }


                    #endregion

                }
                return executeResult;
            }
            else
            {
                autoSubmitResult.DebugMessages = "后续步骤未找到接收人";
                autoSubmitResult.IsSuccess = false;
                autoSubmitResult.Messages = "后续步骤未找到接收人";
                return autoSubmitResult;
            }
        }


        /// <summary>
        /// 转交任务
        /// </summary>
        /// <param name="executeModel"></param>
        private void executeRedirect(WorkFlowExecute executeModel)
        {
            var currentTask = _workFlowTaskRepository.Get(executeModel.TaskID);
            var currentUser = _useRepository.Get(AbpSession.UserId.Value);
            if (currentTask == null)
            {
                result.DebugMessages = "未能创建或找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未能创建或找到当前任务";
                return;
            }
            else if (currentTask.Status.In(2, 3, 4, 5, 6, 7))
            {
                result.DebugMessages = "当前任务已处理";
                result.IsSuccess = false;
                result.Messages = "当前任务已处理";
                return;
            }
            else if (currentTask.ReceiveID != currentUser.Id)
            {
                result.DebugMessages = "不能处理当前任务";
                result.IsSuccess = false;
                result.Messages = "不能处理当前任务";
                return;
            }
            else if (currentTask.Status == -1)
            {
                result.DebugMessages = "当前任务正在等待他人处理";
                result.IsSuccess = false;
                result.Messages = "当前任务正在等待他人处理";
                return;
            }
            if (executeModel.Steps.First().Value.Count == 0)
            {
                result.DebugMessages = "未设置转交人员";
                result.IsSuccess = false;
                result.Messages = "未设置转交人员";
                return;
            }
            string receiveName = currentTask.ReceiveName;
            foreach (var user in executeModel.Steps.First().Value)
            {
                var newTask = new WorkFlowTask();
                newTask.Id = Guid.NewGuid();
                newTask.FlowID = currentTask.FlowID;
                newTask.GroupID = currentTask.GroupID;
                newTask.InstanceID = currentTask.InstanceID;
                newTask.PrevID = currentTask.PrevID;
                newTask.PrevStepID = currentTask.PrevStepID;
                newTask.SenderID = currentTask.SenderID;
                newTask.SenderName = currentTask.SenderName;
                newTask.SenderTime = currentTask.SenderTime;
                newTask.Sort = currentTask.Sort;
                newTask.StepID = currentTask.StepID;
                newTask.StepName = currentTask.StepName;
                newTask.Title = currentTask.Title;
                newTask.VersionNum = currentTask.VersionNum;

                newTask.ReceiveID = user.Id;
                newTask.ReceiveName = user.Name;
                newTask.OpenTime = null;
                newTask.Status = 0;
                newTask.IsSign = 0;
                newTask.Type = 3;
                newTask.Deepth = currentTask.Deepth;
                newTask.OtherType = currentTask.OtherType;

                newTask.Note = string.Format("该任务由{0}转交", receiveName);
                if (!HasNoCompletedTasks(currentTask.FlowID, currentTask.StepID, currentTask.GroupID, user.Id))
                {
                    _workFlowTaskRepository.Insert(newTask);
                }
                nextTasks.Add(newTask);
            }
            Completed(executeModel.TaskID, executeModel.Comment, executeModel.IsSign, 2, "已转交他人处理");

            var nextUserName = new List<string>();
            foreach (var user in executeModel.Steps.First().Value)
            {
                nextUserName.Add(user.Name);
            }
            string userName = nextUserName.Distinct().ToArray().Join1(",");
            result.DebugMessages = string.Concat("已转交给:", userName);
            result.IsSuccess = true;
            result.Messages = string.Concat("已转交给:", userName);
            return;
        }


        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="executeModel"></param>
        private void executeAddWrite(WorkFlowExecute executeModel)
        {
            if (executeModel.TaskID.IsEmptyGuid())
            {
                result.DebugMessages = "未找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未找到当前任务";
                return;
            }
            var task = _workFlowTaskRepository.Get(executeModel.TaskID);
            var currentUser = _useRepository.Get(AbpSession.UserId.Value);
            if (task == null)
            {
                result.DebugMessages = "未找到当前任务";
                result.IsSuccess = false;
                result.Messages = "未找到当前任务";
                return;
            }
            else if (task.ReceiveID != currentUser.Id)
            {
                result.DebugMessages = "不能处理当前任务";
                result.IsSuccess = false;
                result.Messages = "不能处理当前任务";
                return;
            }
            if (task.OtherType.ToString().Length != 2)
            {
                result.DebugMessages = "未找到加签类型和审批类型!";
                result.IsSuccess = false;
                result.Messages = "加签参数错误";
                return;
            }
            int addType = task.OtherType.ToString().Left(1).ToInt();
            int writeType = task.OtherType.ToString().Right(1).ToInt();
            var stepTasks = GetTaskList1(task.FlowID, task.StepID, task.GroupID);
            var tasks = stepTasks.FindAll(p => p.PrevID == task.PrevID && p.Type == 7);
            var nextTasks1 = new List<WorkFlowTask>();
            switch (writeType)
            {
                case 1://所有人同意
                    Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                    break;
                case 2://一人同意即可
                    foreach (var t in tasks.FindAll(p => p.Status.In(-1, 0, 1)))
                    {
                        if (t.Id == task.Id)
                        {
                            Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                        }
                        else
                        {
                            Completed(t.Id, "", false, 4);
                        }
                    }
                    break;
                case 3://顺序审批
                    Completed(task.Id, executeModel.Comment, executeModel.IsSign, files: executeModel.Files);
                    var nextTasks = tasks.FindAll(p => p.Status.In(-1, 0, 1) && p.Id != task.Id).OrderBy(p => p.ReceiveTime);
                    if (nextTasks.Any())
                    {
                        var nextTask = nextTasks.FirstOrDefault();
                        nextTask.Status = 0;
                        _workFlowTaskRepository.Update(nextTask);
                        nextTasks1.Add(nextTask);
                    }
                    break;
            }
            var noAddWriteTasks = new List<WorkFlowTask>();
            bool isPassing = isPassingAddWrite(task, out noAddWriteTasks);

            if (isPassing && noAddWriteTasks.Count > 0)
            {
                switch (addType)
                {
                    case 1://前加签
                        foreach (var t in noAddWriteTasks)
                        {
                            t.Status = 1;
                            _workFlowTaskRepository.Update(t);
                            nextTasks1.Add(t);
                        }
                        break;
                    case 2://后加签
                    case 3://并签
                        var nextTasks = GetNextTaskList(noAddWriteTasks.FirstOrDefault().Id).FindAll(p => p.Status == -1);
                        foreach (var nextTask in nextTasks)
                        {
                            nextTask.Status = 0;
                            _workFlowTaskRepository.Update(nextTask);
                            nextTasks1.Add(nextTask);
                        }
                        break;
                }
            }
            StringBuilder sendName = new StringBuilder();
            foreach (var nextTask in nextTasks1)
            {
                sendName.Append(nextTask.ReceiveName);
                sendName.Append(",");
            }

            result.DebugMessages = "已发送" + (sendName.Length > 0 ? "到" + sendName.ToString().TrimEnd(',') : "");
            result.IsSuccess = true;
            result.NextTasks = nextTasks1;
            result.Messages = result.DebugMessages;
        }



        public Dictionary<Guid, string> GetBackSteps(Guid taskID, int backType, Guid stepID, WorkFlowInstalled wfInstalled)
        {
            Dictionary<Guid, string> dict = new Dictionary<Guid, string>();
            var steps = wfInstalled.Steps.Where(p => p.ID == stepID);
            if (steps.Count() == 0)
            {
                return dict;
            }
            var step = steps.First();
            var task = _workFlowTaskRepository.Get(taskID);
            //加签退回给发送人
            if (task != null && task.Type == 7)
            {
                dict.Add(Guid.Empty, task.SenderName);
                return dict;
            }
            switch (backType)
            {
                case 0://退回前一步
                    if (task != null)
                    {
                        if (step.Behavior.Countersignature != 0)//如果是会签步骤，则要退回到前面所有步骤
                        {
                            var backSteps = GetPrevSteps(task.FlowID, step.ID);
                            foreach (var backStep in backSteps)
                            {
                                dict.Add(backStep.ID, backStep.Name);
                            }
                        }
                        else
                        {
                            dict.Add(task.PrevStepID, GetStepName(task.PrevStepID, wfInstalled));
                        }
                    }
                    break;
                case 1://退回第一步
                    dict.Add(wfInstalled.FirstStepID, GetStepName(wfInstalled.FirstStepID, wfInstalled));
                    break;
                case 2://退回某一步
                    if (step.Behavior.BackType == 2 && step.Behavior.BackStepID.HasValue && step.Behavior.BackStepID.Value != Guid.Empty)
                    {
                        dict.Add(step.Behavior.BackStepID.Value, GetStepName(step.Behavior.BackStepID.Value, wfInstalled));
                    }
                    else
                    {
                        if (task != null)
                        {
                            var taskList = _workFlowTaskRepository.GetAll().Where(r => r.FlowID == task.FlowID && r.GroupID == task.GroupID).Where(p => p.Status.In(2, 3, 4)).OrderBy(p => p.Sort);
                            foreach (var task1 in taskList.OrderByDescending(p => p.CompletedTime1))
                            {
                                if (!dict.Keys.Contains(task1.StepID) && task1.StepID != stepID)
                                {
                                    dict.Add(task1.StepID, GetStepName(task1.StepID, wfInstalled));
                                }
                            }
                        }
                    }
                    break;
            }
            return dict;
        }


        /// <summary>
        /// 根据步骤ID得到步骤名称
        /// </summary>
        /// <param name="stepID"></param>
        /// <param name="flowID"></param>
        /// <param name="flowName"></param>
        /// <param name="defaultFirstStepName">如果步骤为空是否返回第一步的名称</param>
        /// <returns></returns>
        public string GetStepName(Guid stepID, Guid flowID, out string flowName, bool defaultFirstStepName = false)
        {
            flowName = "";
            var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
            if (wfInstalled == null) return "";
            if (stepID == Guid.Empty && defaultFirstStepName)
            {
                stepID = wfInstalled.FirstStepID;
            }
            flowName = wfInstalled.Name;
            var steps = wfInstalled.Steps.Where(p => p.ID == stepID);
            return steps.Count() > 0 ? steps.First().Name : "";
        }

        /// <summary>
        /// 根据步骤ID得到步骤名称
        /// </summary>
        /// <param name="stepID"></param>
        /// <param name="flowID"></param>
        /// <param name="defautFirstStepName">如果步骤ID为空是否默认为第一步</param>
        /// <returns></returns>
        public string GetStepName(Guid stepID, Guid flowID, bool defautFirstStepName = false)
        {
            string temp;
            return GetStepName(stepID, flowID, out temp, defautFirstStepName);
        }
        /// <summary>
        /// 根据步骤ID得到步骤名称
        /// </summary>
        /// <param name="stepID"></param>
        /// <param name="flowID"></param>
        /// <param name="defautFirstStepName">如果步骤ID为空是否默认为第一步</param>
        /// <returns></returns>
        public string GetStepName(Guid stepID, WorkFlowInstalled wfinstalled, bool defautFirstStepName = false)
        {
            if (wfinstalled == null) return "";
            if (stepID == Guid.Empty && defautFirstStepName)
            {
                stepID = wfinstalled.FirstStepID;
            }
            var steps = wfinstalled.Steps.Where(p => p.ID == stepID);
            return steps.Count() > 0 ? steps.First().Name : "";
        }


        public Dictionary<long, string> GetStepHasPassMember(Guid flowid, Guid groupid, Guid stepid, int sort)
        {
            var data = from t in _workFlowTaskRepository.GetAll()
                       where t.FlowID == flowid && t.GroupID == groupid && t.StepID == stepid && t.Sort == sort && t.Status == 2
                       select new { t.ReceiveID, t.ReceiveName };
            var ret = new Dictionary<long, string>();
            ///退了一步 马上再退一步 sort-1的方式不对
            if (data.Count() == 0)
            {
                var query = _workFlowTaskRepository.GetAll().Where(r => r.FlowID == flowid && r.GroupID == groupid && r.StepID == stepid && r.Status == 2);
                var query_list = query.ToList();
                var maxSort = query_list.Max(r => r.Sort);
                var taskList = query.Where(r => r.Sort == maxSort);
                foreach (var item in taskList)
                {
                    ret.Add(item.ReceiveID, item.ReceiveName);
                }
            }
            else
            {
                foreach (var item in data)
                {
                    ret.Add(item.ReceiveID, item.ReceiveName);
                }
            }

            return ret;
        }


        #region 帮助方法


        /// <summary>
        /// 删除流程实例
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private int DeleteInstance(Guid flowID, Guid groupID, bool hasInstanceData = false)
        {
            //暂不实现
            return 1;
        }


        private List<WorkFlowTask> GetTaskList1(Guid flowID, Guid stepID, Guid groupID)
        {
            var query = from t in _workFlowTaskRepository.GetAll()
                        where t.StepID == stepID && t.GroupID == groupID
                        select t;
            var ret = new List<WorkFlowTask>();
            query.ToList().ForEach(r => ret.Add(r));
            return ret;

        }

        private List<WorkFlowTask> GetNextTaskList(Guid taskID)
        {
            var query = from t in _workFlowTaskRepository.GetAll()
                        where t.PrevID == taskID
                        select t;
            var retdata = new List<WorkFlowTask>();
            query.ToList().ForEach(r => retdata.Add(r));
            return retdata;

        }


        /// <summary>
        /// 得到一个流程当前步骤的后续步骤集合
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <returns></returns>
        public List<WorkFlowStep> GetNextSteps(Guid flowID, Guid stepID, int? versionNumber = null)
        {
            var stepList = new List<WorkFlowStep>();
            var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID, versionNumber);
            if (wfInstalled == null)
            {
                return stepList;
            }
            var lines = wfInstalled.Lines.Where(p => p.FromID == stepID);
            foreach (var line in lines)
            {
                var step = wfInstalled.Steps.Where(p => p.ID == line.ToID);
                if (step.Count() > 0)
                {
                    stepList.Add(step.First());
                }
            }
            return stepList;
        }


        /// <summary>
        /// 得到一个流程步骤的前面步骤集合
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <returns></returns>
        private List<WorkFlowStep> GetPrevSteps(Guid flowID, Guid stepID)
        {
            var stepList = new List<WorkFlowStep>();
            if (wfInstalled == null)
            {
                return stepList;
            }
            var lines = wfInstalled.Lines.Where(p => p.ToID == stepID);
            foreach (var line in lines)
            {
                var step = wfInstalled.Steps.Where(p => p.ID == line.FromID);
                if (step.Any())
                {
                    stepList.Add(step.First());
                }
            }
            return stepList;
        }


        /// 得到和一个任务同级的任务
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="isStepID">是否区分步骤ID，多步骤会签区分的是上一步骤ID</param>
        /// <returns></returns>
        public List<WorkFlowTask> GetTaskList2(Guid taskID, bool isStepID = true)
        {
            var task = _workFlowTaskRepository.Get(taskID);
            if (task == null)
            {
                return new List<WorkFlowTask>() { };
            }
            var query = from t in _workFlowTaskRepository.GetAll()
                        where t.FlowID == task.FlowID && t.GroupID == task.GroupID && t.PrevID == task.PrevID
                        select t;
            query = query.WhereIf(isStepID, r => r.StepID == task.StepID).WhereIf(!isStepID, r => r.PrevStepID == task.PrevStepID);
            var ret = new List<WorkFlowTask>();
            query.ToList().ForEach(r => ret.Add(r));
            return ret;
        }

        /// <summary>
        /// 得到一个实例的任务
        /// </summary>
        /// <param name="flowID">flowID为空，则只根据groupID查询</param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<WorkFlowTask> GetTaskList3(Guid flowID, Guid groupID)
        {
            var ret = new List<WorkFlowTask>();
            var query =
                _workFlowTaskRepository.GetAll()
                    .Where(r => r.GroupID == groupID)
                    .WhereIf(flowID != Guid.Empty, r => r.FlowID == flowID);
            foreach (var workFlowTask in query)
            {
                ret.Add(workFlowTask);
            }
            return ret;

        }

        public List<WorkFlowTask> GetTaskList4(string instanceId, Guid flowID, Guid? groupID)
        {
            var ret = new List<WorkFlowTask>();
            var query =
                _workFlowTaskRepository.GetAll()
                    .Where(r => r.InstanceID == instanceId)
                    .Where(r => r.FlowID == flowID)
                    .WhereIf(groupID.HasValue, r => r.GroupID == groupID.Value).OrderBy(x => x.ReceiveTime);
            foreach (var workFlowTask in query)
            {
                ret.Add(workFlowTask);
            }
            return ret;

        }

        /// <summary>
        /// 删除一组实例
        /// </summary>
        private int Delete(Guid flowID, Guid groupID)
        {
            try
            {
                var query =
                _workFlowTaskRepository.GetAll()
                    .Where(r => r.GroupID == groupID)
                    .WhereIf(flowID != Guid.Empty, r => r.FlowID == flowID);
                foreach (var workFlowTask in query)
                {
                    _workFlowTaskRepository.Delete(workFlowTask);
                }
                return query.Count();
            }
            catch (Exception e)
            {
                return 0;
            }

        }


        /// <summary>
        /// 删除临时任务
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <param name="groupID"></param>
        /// <param name="prevStepID"></param>
        /// <returns></returns>
        private int DeleteTempTasks(Guid flowID, Guid stepID, Guid groupID, Guid prevStepID)
        {
            try
            {
                _workFlowTaskRepository.Delete(
                    r => r.FlowID == flowID && r.StepID == stepID && r.GroupID == groupID && r.Status == -1);
                return 1;

            }
            catch (Exception e)
            {
                return 0;
            }
        }



        /// <summary>
        /// 判断一个步骤是否退回
        /// </summary>
        /// <param name="step"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private bool IsBack(WorkFlowStep step, Guid flowID, Guid groupID, Guid taskID, int sort)
        {
            var tasks = GetTaskList1(flowID, step.ID, groupID).FindAll(p => p.Sort == sort && p.Type != 5);
            if (tasks.Count == 0)
            {
                return false;
            }
            bool isBack = true;
            switch (step.Behavior.HanlderModel)
            {
                case 0://所有人必须处理
                case 3://独立处理
                    isBack = tasks.Any(p => p.Status.In(3, 5));
                    break;
                case 1://一人同意即可
                    isBack = !tasks.Any(p => p.Status.In(2, 4));
                    break;
                case 2://依据人数比例
                    isBack = (((decimal)(tasks.Count(p => p.Status.In(3, 5)) + 1) / (decimal)tasks.Count) * 100).Round() >= 100 - (step.Behavior.Percentage <= 0 ? 100 : step.Behavior.Percentage);
                    break;
            }
            return isBack;
        }


        /// <summary>
        /// 判断实例是否已完成
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool GetInstanceIsCompleted(Guid flowID, Guid groupID)
        {
            var tasks = GetTaskList3(Guid.Empty, groupID);
            return tasks.Find(p => p.Status.In(0, 1)) == null;
        }


        /// <summary>
        /// 判断实例是否已完成 排除抄送任务
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool GetInstanceIsCompletedWithOutCopyTask(Guid flowID, Guid groupID)
        {
            var tasks = GetTaskList3(Guid.Empty, groupID);
            return tasks.Find(p => p.Status.In(0, 1) && p.Type != 5) == null;
        }


        /// <summary>
        /// 根据SubFlowID得到一个任务
        /// </summary>
        /// <param name="subflowGroupID"></param>
        /// <returns></returns>
        public List<WorkFlowTask> GetBySubFlowGroupID(Guid subflowGroupID)
        {
            var ret = new List<WorkFlowTask>();
            var query = _workFlowTaskRepository.GetAll().Where(r => r.SubFlowGroupID.Contains(subflowGroupID.ToString()));
            foreach (var workFlowTask in query)
            {
                ret.Add(workFlowTask);
            }
            return ret;
        }


        /// <summary>
        /// 得到一个步骤的默认处理人员
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <param name="groupID"></param>
        /// <param name="currentStepID"></param>
        /// <param name="instanceid"></param>
        /// <param name="selectType">选择类型</param>
        /// <param name="selectRange">选择范围</param>
        /// <returns></returns>
        public string GetDefultMember(Guid flowID, Guid stepID, Guid groupID, Guid currentStepID, string instanceid, out string selectType, out string selectRange, Guid currenttaskId, int? versionNum = null)
        {
            var defaultMember = string.Empty;//默认处理人员
            selectType = "";
            selectRange = "";
            var wfInstalled1 = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID, versionNum);
            if (wfInstalled1 == null)
            {
                return defaultMember;
            }
            var steps = wfInstalled1.Steps.Where(p => p.ID == stepID);
            if (!steps.Any())
            {
                return defaultMember;
            }
            var step = steps.First();
            //如果是调试模式并且当前登录人员包含在调试人员中 则默认为发起者
            if ((wfInstalled1.Debug == 1 || wfInstalled1.Debug == 2) && wfInstalled1.DebugUsers.Exists(p => p.Id == AbpSession.UserId.Value))
            {
                defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
            }
            else
            {
                var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
                switch (step.Behavior.HandlerType)
                {
                    case 0:  //所有成员
                        selectType = "unit='1' dept='1' station='1' workgroup='1' user='1'";
                        break;
                    case 1: //部门领导
                        selectType = "unit='0' dept='1' station='0' workgroup='0' user='0'";
                        if (step.Behavior.SelelctOrgIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = step.Behavior.SelelctOrgIds.Split(",").ToList();
                            for (var i = 0; i < rangArrys.Count(); i++)
                            {
                                rangArrys[i] = $"{MemberPerfix.DepartmentPREFIX}{rangArrys[i]}";
                            }
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 2: //岗位
                        selectType = "unit='0' dept='0' station='1' workgroup='0' user='0'";
                        if (step.Behavior.SelectPostIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = step.Behavior.SelectPostIds.Split(",").ToList();
                            for (var i = 0; i < rangArrys.Count(); i++)
                            {
                                rangArrys[i] = $"{MemberPerfix.PostPREFIX}{rangArrys[i]}";
                            }
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 3: //工作组
                        selectType = "unit='0' dept='0' station='0' workgroup='1' user='0'";
                        break;
                    case 4: ///人员
                        selectType = "unit='0' dept='0' station='0' workgroup='0' user='1'";
                        break;
                    case 5://发起者
                        var userid = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (userid != 0)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + userid.ToString();
                        }
                        if (defaultMember.IsNullOrEmpty() && currentStepID == wfInstalled1.FirstStepID)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        break;
                    case 6://前一步骤处理者
                        defaultMember = GetStepSnderIDString(flowID, currentStepID, groupID);
                        if (defaultMember.IsNullOrEmpty() && currentStepID == wfInstalled.FirstStepID)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        break;
                    case 7://某一步骤处理者
                        defaultMember = GetStepSnderIDString(wfInstalled1.ID, step.Behavior.HandlerStepID.Value, groupID);
                        if (defaultMember.IsNullOrEmpty() && step.Behavior.HandlerStepID.Value == wfInstalled1.FirstStepID)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        break;
                    case 8://字段值
                        string linkString = step.Behavior.ValueField;
                        if (!linkString.IsNullOrEmpty() && !instanceid.IsNullOrEmpty() && wfInstalled1.DataBases.Any())
                        {
                            var defaultMemberSql = GetFieldValueSqlWithDefaultMemberModel(linkString, wfInstalled1.DataBases.First().PrimaryKey, instanceid);
                            defaultMember = _workFlowTaskRepository.GetDefaultMemberExecuteQuery(defaultMemberSql).Key;
                            if (string.IsNullOrWhiteSpace(defaultMember))
                            {
                                return "";
                            }
                        }
                        break;
                    case 9://发起者分管领导
                        var firstSenderID = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID = AbpSession.UserId.Value;
                        }
                        if (firstSenderID != 0)
                        {

                            defaultMember = organizeManager.GetChargeLeader(firstSenderID);
                        }
                        break;
                    case 10://发起者部门领导
                        var firstSenderID1 = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID1 == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID1 = AbpSession.UserId.Value;
                        }
                        if (firstSenderID1 != 0)
                        {
                            defaultMember = organizeManager.GetLeader(firstSenderID1);
                        }
                        break;
                    case 11://前一步处理者领导
                        defaultMember = organizeManager.GetLeader(AbpSession.UserId.Value);
                        break;
                    case 12://前一步处理者分管领导
                        defaultMember = organizeManager.GetChargeLeader(AbpSession.UserId.Value);
                        break;
                    case 13://发起者上级部门领导
                        var firstSenderID2 = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID2 == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID2 = AbpSession.UserId.Value;
                        }
                        if (firstSenderID2 != 0)
                        {
                            defaultMember = organizeManager.GetParentDeptLeader(firstSenderID2);
                        }
                        break;
                    case 14://前一步处理者上级部门领导
                        defaultMember = organizeManager.GetParentDeptLeader(AbpSession.UserId.Value);
                        break;
                    case 15://前一步处理者部门所有成员
                        string userString = GetStepSnderIDString(wfInstalled1.ID, currentStepID, groupID);
                        if (userString.IsNullOrEmpty() && step.Behavior.HandlerStepID.Value == wfInstalled1.FirstStepID)
                        {
                            userString = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        StringBuilder sb = new StringBuilder();
                        foreach (string user in (userString ?? "").Split(','))
                        {
                            var dept = organizeManager.GetDeptByUserID(MemberPerfix.RemovePrefix(user).ToLong());
                            if (dept != null)
                            {
                                var users1 = organizeManager.GetAllUsersById(dept.Id);
                                foreach (var u in users1)
                                {
                                    sb.Append("u_");
                                    sb.Append(u.Id);
                                    sb.Append(",");
                                }
                            }
                        }
                        defaultMember = sb.ToString().TrimEnd(',');
                        break;
                    case 16://发起者部门所有成员
                        var firstSenderID3 = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID3 == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID3 = AbpSession.UserId.Value;
                        }

                        StringBuilder sb1 = new StringBuilder();
                        var dept1 = organizeManager.GetDeptByUserID(firstSenderID3);
                        if (dept1 != null)
                        {
                            var users1 = organizeManager.GetAllUsersById(dept1.Id);
                            foreach (var u in users1)
                            {
                                sb1.Append(MemberPerfix.UserPREFIX);
                                sb1.Append(u.Id);
                                sb1.Append(",");
                            }
                        }
                        defaultMember = sb1.ToString().TrimEnd(',');
                        break;
                    case 17://自定义方法
                        //if (!currenttaskId.HasValue) break;
                        var eventParams = new WorkFlowCustomEventParams();
                        eventParams.FlowID = flowID;
                        eventParams.GroupID = groupID;
                        eventParams.StepID = stepID;
                        eventParams.TaskID = currenttaskId;
                        eventParams.InstanceID = instanceid;
                        //var btask = new WorkFlowTask();

                        //var dllname = "";
                        //if (step.Behavior.CustomEvent.Contains("ZCYX.FRMS.AbpZeroTemplate.Web"))
                        //    dllname = "ZCYX.FRMS.AbpZeroTemplate.Web";
                        //defaultMember = ExecuteFlowCustomEvent(step.Behavior.CustomEvent, eventParams, dllname).ToString();
                        //自定义方法获取处理人


                        if (step.Behavior.CustomEvent.StartsWith("$"))
                        {
                            defaultMember = ExecuteFlowCustomEvent(step.Behavior.CustomEvent, eventParams).ToString();
                        }
                        else
                        {
                            var retuserIds = ExecuteFlowCustomEventByDapperRetList(step.Behavior.CustomEvent, eventParams);
                            if (retuserIds != null)
                            {
                                foreach (var retuserId in retuserIds)
                                {
                                    if (!defaultMember.IsNullOrWhiteSpace())
                                        defaultMember = defaultMember + ",";
                                    defaultMember = defaultMember + retuserId.Id;
                                }
                            }
                        }

                        //defaultMember = ExecuteFlowCustomEventByDapper(step.Behavior.CustomEvent, eventParams).ToString();
                        //defaultMember = sb1.ToString().TrimEnd(',');
                        break;
                    case 18://角色测试
                        if (step.Behavior.RoleId > 0)
                            defaultMember = organizeManager.GetAbpUserIdByRoleId((int)step.Behavior.RoleId).ToString().TrimEnd(',');
                        break;
                    case 19://部门直属成员
                        if (step.Behavior.SelelctOrgIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = step.Behavior.SelelctOrgIds.Split(",").ToList();
                            rangArrys.ForEach(r => r = $"{MemberPerfix.DepartmentMemberPREFIX}{r}");
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 20://部门所有成员
                        if (step.Behavior.SelelctOrgIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = step.Behavior.SelelctOrgIds.Split(",").ToList();
                            rangArrys.ForEach(r => r = $"{MemberPerfix.AllUserPREFIX}{r}");
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 21://当前处理者
                        defaultMember = "u_" + AbpSession.UserId.Value;
                        break;
                    case 22://部门分管领导
                        defaultMember = "u_" + AbpSession.UserId.Value;
                        break;
                }
            }
            if (!defaultMember.IsNullOrEmpty())
            {
                defaultMember = defaultMember.Split(',').Distinct().ToArray().Join1();
            }
            if (step.Behavior.HandlerType.In(9, 10, 11, 12, 13, 14, 15, 16))
            {
                selectRange = "rootid=\"" + defaultMember + "\"";
            }

            if (defaultMember.IsNullOrEmpty())
            {
                defaultMember = step.Behavior.DefaultHandler;
            }

            return defaultMember;
        }

        /// <summary>
        /// 得到一个步骤的默认处理人员
        /// </summary>
        /// <param name="type">1,接受抄送2，处理抄送</param>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <param name="groupID"></param>
        /// <param name="currentStepID"></param>
        /// <param name="instanceid"></param>
        /// <param name="selectType">选择类型</param>
        /// <param name="selectRange">选择范围</param>
        /// <returns></returns>
        public string GetDefultMemberByCopyFor(int type, Guid flowID, Guid stepID, Guid groupID, Guid currentStepID, string instanceid, out string selectType, out string selectRange, Guid currenttaskId, int? versionNum = null)
        {
            var defaultMember = string.Empty;//默认处理人员
            selectType = "";
            selectRange = "";
            var wfInstalled1 = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID, versionNum);
            if (wfInstalled1 == null)
            {
                return defaultMember;
            }
            var steps = wfInstalled1.Steps.Where(p => p.ID == stepID);
            if (!steps.Any())
            {
                return defaultMember;
            }
            var step = steps.First();
            //如果是调试模式并且当前登录人员包含在调试人员中 则默认为发起者
            if ((wfInstalled1.Debug == 1 || wfInstalled1.Debug == 2) && wfInstalled1.DebugUsers.Exists(p => p.Id == AbpSession.UserId.Value))
            {
                defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
            }
            else
            {
                var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
                var handlerType = 0;
                if (type == 1)
                    handlerType = Convert.ToInt32(step.Behavior.CopyForHandlerType);
                if (type == 2)
                    handlerType = Convert.ToInt32(step.Behavior.CopyForSendHandlerType);
                var handlerStepID = Guid.Empty;

                if (type == 1 && step.Behavior.CopyForHandlerStepID != null)
                    handlerStepID = step.Behavior.CopyForHandlerStepID.Value;

                if (type == 2 && step.Behavior.CopyForSendHandlerStepID != null)
                    handlerStepID = step.Behavior.CopyForSendHandlerStepID.Value;
                var orgIds = step.Behavior.CopyForSelelctOrgIds;
                if (type == 2)
                    orgIds = step.Behavior.CopyForSendSelelctOrgIds;
                switch (handlerType)
                {
                    case 0:
                        selectType = "unit='1' dept='1' station='1' workgroup='1' user='1'";
                        break;
                    case 1: //部门领导
                        selectType = "unit='0' dept='1' station='0' workgroup='0' user='0'";
                        if (orgIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = orgIds.Split(",").ToList();
                            for (var i = 0; i < rangArrys.Count(); i++)
                            {
                                rangArrys[i] = $"{MemberPerfix.DepartmentPREFIX}{rangArrys[i]}";
                            }
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 2: //岗位
                        selectType = "unit='0' dept='0' station='1' workgroup='0' user='0'";
                        var postIds = step.Behavior.CopyForSelectPostIds;
                        if (type == 2)
                            postIds = step.Behavior.CopyForSendSelectPostIds;
                        if (postIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = postIds.Split(",").ToList();
                            for (var i = 0; i < rangArrys.Count(); i++)
                            {
                                rangArrys[i] = $"{MemberPerfix.PostPREFIX}{rangArrys[i]}";
                            }
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 3:
                        selectType = "unit='0' dept='0' station='0' workgroup='1' user='0'";
                        break;
                    case 4:
                        selectType = "unit='0' dept='0' station='0' workgroup='0' user='1'";
                        break;
                    case 5://发起者
                        var userid = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (userid != 0)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + userid.ToString();
                        }
                        if (defaultMember.IsNullOrEmpty() && currentStepID == wfInstalled1.FirstStepID)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        break;
                    case 6://前一步骤处理者
                        defaultMember = GetStepSnderIDString(flowID, currentStepID, groupID);
                        if (defaultMember.IsNullOrEmpty() && currentStepID == wfInstalled.FirstStepID)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        break;
                    case 7://某一步骤处理者
                        defaultMember = GetStepSnderIDString(wfInstalled1.ID, handlerStepID, groupID);
                        if (defaultMember.IsNullOrEmpty() && handlerStepID == wfInstalled1.FirstStepID)
                        {
                            defaultMember = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        break;
                    case 8://字段值
                        string linkString = step.Behavior.CopyForValueField;
                        if (type == 2)
                            linkString = step.Behavior.CopyForSendValueField;
                        if (!linkString.IsNullOrEmpty() && !instanceid.IsNullOrEmpty() && wfInstalled1.DataBases.Any())
                        {
                            var defaultMemberSql = GetFieldValueSqlWithDefaultMemberModel(linkString, wfInstalled1.DataBases.First().PrimaryKey, instanceid);
                            defaultMember = _workFlowTaskRepository.GetDefaultMemberExecuteQuery(defaultMemberSql).Key;
                            var leader = string.Empty;
                            var list = defaultMember.Split(',').Where(p => p.Contains("l_")).ToList();
                            //foreach (var m in list)
                            //{
                            //    var dept = _organizationUnitRepository.Get(long.Parse(m.Replace("l_", "")));
                            //    leader += dept.ChargeLeader + ",";
                            //    defaultMember = defaultMember.Replace(m, "");
                            //}

                            // 改为： 通过岗位获取领导
                            foreach (var m in list)
                            {
                                var orgid = long.Parse(m.Replace("l_", ""));
                                var leaderuserid = organizeManager.GetChargeLeaderByIds(orgid);
                                leader += leaderuserid + ",";
                                defaultMember = defaultMember.Replace(m, "");
                            }
                            defaultMember = defaultMember + leader.Trim(',');
                        }
                        break;
                    case 9://发起者主管
                        var firstSenderID = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID = AbpSession.UserId.Value;
                        }
                        if (firstSenderID != 0)
                        {

                            defaultMember = organizeManager.GetLeader(firstSenderID);
                        }
                        break;
                    case 10://发起者分管领导
                        var firstSenderID1 = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID1 == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID1 = AbpSession.UserId.Value;
                        }
                        if (firstSenderID1 != 0)
                        {
                            defaultMember = organizeManager.GetChargeLeader(firstSenderID1);
                        }
                        break;
                    case 11://前一步处理者领导
                        defaultMember = organizeManager.GetLeader(AbpSession.UserId.Value);
                        break;
                    case 12://前一步处理者分管领导
                        defaultMember = organizeManager.GetChargeLeader(AbpSession.UserId.Value);
                        break;
                    case 13://发起者上级部门领导
                        var firstSenderID2 = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID2 == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID2 = AbpSession.UserId.Value;
                        }
                        if (firstSenderID2 != 0)
                        {
                            defaultMember = organizeManager.GetParentDeptLeader(firstSenderID2);
                        }
                        break;
                    case 14://前一步处理者上级部门领导
                        defaultMember = organizeManager.GetParentDeptLeader(AbpSession.UserId.Value);
                        break;
                    case 15://前一步处理者部门所有成员
                        string userString = GetStepSnderIDString(wfInstalled1.ID, currentStepID, groupID);
                        if (userString.IsNullOrEmpty() && handlerStepID == wfInstalled1.FirstStepID)
                        {
                            userString = MemberPerfix.UserPREFIX + AbpSession.UserId.Value.ToString();
                        }
                        StringBuilder sb = new StringBuilder();
                        foreach (string user in (userString ?? "").Split(','))
                        {
                            var dept = organizeManager.GetDeptByUserID(MemberPerfix.RemovePrefix(user).ToLong());
                            if (dept != null)
                            {
                                var users1 = organizeManager.GetAllUsersById(dept.Id);
                                foreach (var u in users1)
                                {
                                    sb.Append("u_");
                                    sb.Append(u.Id);
                                    sb.Append(",");
                                }
                            }
                        }
                        defaultMember = sb.ToString().TrimEnd(',');
                        break;
                    case 16://发起者部门所有成员
                        var firstSenderID3 = GetFirstSnderID(wfInstalled1.ID, groupID);
                        if (firstSenderID3 == 0 && currentStepID == wfInstalled1.FirstStepID)//如果是第一步则发起者为当前人员
                        {
                            firstSenderID3 = AbpSession.UserId.Value;
                        }

                        StringBuilder sb1 = new StringBuilder();
                        var dept1 = organizeManager.GetDeptByUserID(firstSenderID3);
                        if (dept1 != null)
                        {
                            var users1 = organizeManager.GetAllUsersById(dept1.Id);
                            foreach (var u in users1)
                            {
                                sb1.Append("u_");
                                sb1.Append(u.Id);
                                sb1.Append(",");
                            }
                        }
                        defaultMember = sb1.ToString().TrimEnd(',');
                        break;
                    case 17://自定义方法
                        //if (!currenttaskId.HasValue) break;
                        var eventParams = new WorkFlowCustomEventParams();
                        eventParams.FlowID = flowID;
                        eventParams.GroupID = groupID;
                        eventParams.StepID = stepID;
                        eventParams.TaskID = currenttaskId;
                        eventParams.InstanceID = instanceid;
                        //var btask = new WorkFlowTask();

                        //var dllname = "";
                        //if (step.Behavior.CustomEvent.Contains("ZCYX.FRMS.AbpZeroTemplate.Web"))
                        //    dllname = "ZCYX.FRMS.AbpZeroTemplate.Web";
                        //defaultMember = ExecuteFlowCustomEvent(step.Behavior.CustomEvent, eventParams, dllname).ToString();
                        //自定义方法获取处理人
                        var customEvent = step.Behavior.CopyForCustomEvent;
                        if (type == 2)
                            customEvent = step.Behavior.CopyForSendCustomEvent;

                        if (customEvent.StartsWith("$"))
                        {
                            defaultMember = ExecuteFlowCustomEvent(customEvent, eventParams).ToString();
                        }
                        else
                        {
                            var retuserIds = ExecuteFlowCustomEventByDapperRetList(customEvent, eventParams);
                            if (retuserIds != null)
                            {
                                foreach (var retuserId in retuserIds)
                                {
                                    if (!defaultMember.IsNullOrWhiteSpace())
                                        defaultMember = defaultMember + ",";
                                    defaultMember = defaultMember + retuserId.Id;
                                }
                            }
                        }

                        //defaultMember = ExecuteFlowCustomEventByDapper(step.Behavior.CustomEvent, eventParams).ToString();
                        //defaultMember = sb1.ToString().TrimEnd(',');
                        break;
                    case 18://角色测试
                        if (type == 1 && step.Behavior.CopyForRoleId > 0)
                            defaultMember = organizeManager.GetAbpUserIdByRoleId((int)step.Behavior.CopyForRoleId).ToString().TrimEnd(',');
                        if (type == 2 && step.Behavior.CopyForSendRoleId > 0)
                            defaultMember = organizeManager.GetAbpUserIdByRoleId((int)step.Behavior.CopyForSendRoleId).ToString().TrimEnd(',');
                        break;
                    case 19://部门直属成员           
                        if (orgIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = orgIds.Split(",").ToList();
                            rangArrys.ForEach(r => r = $"{MemberPerfix.DepartmentMemberPREFIX}{r}");
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 20://部门所有成员
                        if (orgIds.IsNullOrWhiteSpace())
                            return "";
                        else
                        {
                            var rangArrys = orgIds.Split(",").ToList();
                            rangArrys.ForEach(r => r = $"{MemberPerfix.AllUserPREFIX}{r}");
                            defaultMember = string.Join(",", rangArrys);
                        }
                        break;
                    case 21://当前处理者
                        defaultMember = "u_" + AbpSession.UserId.Value;
                        break;
                }
            }
            if (!defaultMember.IsNullOrEmpty())
            {
                defaultMember = defaultMember.Split(',').Distinct().ToArray().Join1();
            }
            if (step.Behavior.HandlerType.In(9, 10, 11, 12, 13, 14, 15, 16))
            {
                selectRange = "rootid=\"" + defaultMember + "\"";
            }

            if (defaultMember.IsNullOrEmpty())
            {
                defaultMember = step.Behavior.DefaultHandler;
            }

            return defaultMember;
        }

        /// <summary>
        /// 得到一个流程实例的发起者
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public long GetFirstSnderID(Guid flowID, Guid groupID)
        {
            var query =
                _workFlowTaskRepository.GetAll()
                    .FirstOrDefault(r => r.FlowID == flowID && r.GroupID == groupID && r.PrevID == Guid.Empty && r.Status == 2);
            return query?.ReceiveID ?? 0;

        }

        /// <summary>
        /// 得到一个流程实例一个步骤的处理者
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private List<long> GetStepSnderID(Guid flowID, Guid stepID, Guid groupID)
        {
            var ret = new List<long>();
            var query = from t in _workFlowTaskRepository.GetAll()
                        where t.FlowID == flowID && t.StepID == stepID && t.GroupID == groupID
                        select t;
            if (query.Count() == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到指定步骤的处理者，该步骤不存在，可能是流程已修改。");
            }
            var data = query.ToList();

            var maxSort = data.Max(o => o.Sort);
            var result = data.Where(r => r.Sort == maxSort);
            foreach (var workFlowTask in result)
            {
                ret.Add(workFlowTask.ReceiveID);
            }
            return ret;
        }

        /// <summary>
        /// 得到一个流程实例一个步骤的处理者字符串
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private string GetStepSnderIDString(Guid flowID, Guid stepID, Guid groupID)
        {
            var list = GetStepSnderID(flowID, stepID, groupID);
            StringBuilder sb = new StringBuilder(list.Count * 43);
            foreach (var li in list)
            {
                sb.Append(MemberPerfix.UserPREFIX);
                sb.Append(li);
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 判断加签是否通过
        /// </summary>
        /// <param name="addWriteTasks"></param>
        /// <param name="isAll">判断当前加签还是步骤所有加签</param>
        /// <returns></returns>
        private bool isPassingAddWrite(WorkFlowTask addWriteTask, out List<WorkFlowTask> nextTasks)
        {
            nextTasks = new List<WorkFlowTask>();
            if (addWriteTask == null)
            {
                return true;
            }
            var tasks = GetTaskList1(addWriteTask.FlowID, addWriteTask.StepID, addWriteTask.GroupID);
            if (tasks.Count == 0)
            {
                return true;
            }
            var task1 = tasks.Find(p => p.Id == addWriteTask.PrevID);
            if (task1 == null)
            {
                return true;
            }
            var tasks1 = tasks.FindAll(p => p.Sort == task1.Sort && p.Type != 7);
            nextTasks = tasks1.FindAll(p => p.Id == addWriteTask.PrevID);
            var addWriteTasks = new List<WorkFlowTask>();
            foreach (var t in tasks1)
            {
                addWriteTasks.AddRange(tasks.FindAll(p => p.PrevID == t.Id && p.Type == 7));
            }

            foreach (var w in addWriteTasks.GroupBy(p => p.PrevID))
            {
                int writeType = w.FirstOrDefault().OtherType.ToString().Right(1).ToInt();
                switch (writeType)
                {
                    case 1:
                    case 3:
                        if (w.Any(p => p.Status.In(0, 1, -1)))
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (!w.Any(p => p.Status.In(2)))
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 创建临时任务（待办人员看不到）
        /// </summary>
        /// <param name="executeModel"></param>
        private List<WorkFlowTask> createTempTasks(WorkFlowExecute executeModel, WorkFlowTask currentTask)
        {
            var tasks = new List<WorkFlowTask>();
            foreach (var step in executeModel.Steps)
            {
                foreach (var user in step.Value)
                {
                    var nextSteps = wfInstalled.Steps.Where(p => p.ID == step.Key);
                    if (!nextSteps.Any())
                    {
                        continue;
                    }
                    var nextStep = nextSteps.First();
                    var task = new WorkFlowTask();
                    if (nextStep.WorkTime > 0)
                    {
                        task.CompletedTime = DateTime.Now.AddHours((double)nextStep.WorkTime);
                    }
                    task.FlowID = executeModel.FlowID;
                    task.GroupID = currentTask?.GroupID ?? executeModel.GroupID;
                    task.Id = Guid.NewGuid();
                    task.Type = 0;
                    task.InstanceID = executeModel.InstanceID;
                    if (!executeModel.Note.IsNullOrEmpty())
                    {
                        task.Note = executeModel.Note;
                    }
                    task.PrevID = currentTask.Id;
                    task.PrevStepID = currentTask.StepID;
                    task.ReceiveID = user.Id;
                    task.ReceiveName = user.Name;
                    task.ReceiveTime = DateTime.Now;
                    task.SenderID = executeModel.Sender.Id;
                    task.SenderName = executeModel.Sender.Name;
                    task.SenderTime = task.ReceiveTime;
                    task.Status = -1;
                    task.StepID = step.Key;
                    task.StepName = nextStep.Name;
                    task.Sort = currentTask.Sort + 1;
                    task.Title = executeModel.Title.IsNullOrEmpty() ? currentTask.Title : executeModel.Title;
                    task.OtherType = executeModel.OtherType;
                    task.VersionNum = executeModel.VersionNum;
                    task.Deepth = currentTask.Deepth;
                    if (!HasNoCompletedTasks(executeModel.FlowID, step.Key, currentTask.GroupID, user.Id))
                    {
                        _workFlowTaskRepository.Insert(task);
                    }
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        /// <summary>
        /// 查询一个用户在一个步骤是否有未完成任务
        /// </summary>
        /// <param name="flowID"></param>
        /// <returns></returns>
        public bool HasNoCompletedTasks(Guid flowID, Guid stepID, Guid groupID, long userID)
        {
            var status = new int[] { -1, 0, 1 };
            var query =
                _workFlowTaskRepository.GetAll()
                    .Where(
                        r =>
                            r.FlowID == flowID && r.StepID == stepID && r.ReceiveID == userID &&
                             r.GroupID == groupID && status.Contains(r.Status));
            return query.Any();


        }

        /// <summary>
        /// 查询一个用户在一个步骤是否有未完成任务
        /// </summary>
        /// <param name="flowID"></param>
        /// <returns></returns>
        private bool HasNoCompletedTasksWithInstanceId(Guid flowID, Guid stepID, string instanceId, long userID)
        {
            var status = new int[] { -1, 0, 1 };
            var query =
                _workFlowTaskRepository.GetAll()
                    .Where(
                        r =>
                            r.FlowID == flowID && r.StepID == stepID && r.ReceiveID == userID &&
                             r.InstanceID == instanceId && status.Contains(r.Status));
            return query.Any();
        }


        /// <summary>
        /// 得到一个流程步骤的前面所有步骤集合
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <returns></returns>
        private List<WorkFlowStep> GetAllPrevSteps(Guid flowID, Guid stepID)
        {
            var stepList = new List<WorkFlowStep>();
            if (wfInstalled == null)
            {
                wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
                if (wfInstalled == null)
                    return stepList;
            }
            addPrevSteps(stepList, wfInstalled, stepID);
            return stepList.Distinct().ToList();
        }

        private void addPrevSteps(List<WorkFlowStep> list, WorkFlowInstalled wfInstalled, Guid stepID)
        {
            if (wfInstalled == null) return;
            var lines = wfInstalled.Lines.Where(p => p.ToID == stepID);
            foreach (var line in lines)
            {
                var step = wfInstalled.Steps.Where(p => p.ID == line.FromID);
                if (step.Any())
                {
                    list.Add(step.First());
                    addPrevSteps(list, wfInstalled, step.First().ID);
                }
            }
        }

        /// <summary>
        /// 判断一个步骤是否通过
        /// </summary>
        /// <param name="step"></param>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <param name="currentTask"></param>
        /// <param name="currentPrevStep">会签步取的上一步的上一步ID</param>
        /// <returns></returns>
        private bool IsPassing(WorkFlowStep step, Guid flowID, Guid groupID, WorkFlowTask currentTask, Guid currentPrevStep)
        {

            var tasks = GetTaskList1(flowID, step.ID, groupID).FindAll(p => p.Type != 7);
            int maxSort = tasks.Count > 0 ? tasks.Max(p => p.Sort) : -1;
            tasks = tasks.FindAll(p => p.Sort == maxSort && p.Type != 5);
            if (tasks.Count == 0)
            {
                //如果步骤没有任务要查询前面的步骤
                var betweenSteps = getBetweenSteps(_workFlowCacheManager.GetWorkFlowModelFromCache(flowID), currentTask.PrevStepID, step.ID);
                foreach (var betweenStep in betweenSteps)
                {
                    //var betweenTasks = GetTaskList1(flowID, betweenStep.ID, groupID).FindAll(p => p.Type != 7 && p.Type != 5);
                    ///排除当前任务  zcl 2018.9.11
                    var betweenTasks = GetTaskList1(flowID, betweenStep.ID, groupID).FindAll(p => p.Id != currentTask.Id && p.Type != 7 && p.Type != 5);
                    int maxsort1 = betweenTasks.Count > 0 ? betweenTasks.Max(p => p.Sort) : -1;//tasks.FindAll(p => p.StepID == betweenStep.ID && p.Type != 5).Max(p => p.Sort);
                    if (betweenTasks.Find(p => p.Sort == maxsort1) != null)
                    {
                        return false;
                    }
                }

                return true;
            }
            bool isPassing = true;
            switch (step.Behavior.HanlderModel)
            {
                case 0://所有人必须处理
                case 3://独立处理
                    isPassing = !tasks.Any(p => p.Status != 2);
                    break;
                case 1://一人同意即可
                    isPassing = tasks.Any(p => p.Status == 2);
                    break;
                case 2://依据人数比例
                    isPassing = (((decimal)(tasks.Count(p => p.Status == 2) + 1) / (decimal)tasks.Count) * 100).Round() >= (step.Behavior.Percentage <= 0 ? 100 : step.Behavior.Percentage);
                    break;
            }
            if (isPassing)
            {
                var addWriteTasks = new List<WorkFlowTask>();
                isPassing = isPassingAddWrite(tasks.FirstOrDefault(), out addWriteTasks);
            }
            return isPassing;
        }



        /// <summary>
        /// 得到流程名称
        /// </summary>
        /// <param name="flowID"></param>
        /// <returns></returns>
        private string GetFlowName(Guid flowID)
        {
            var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
            return flow != null ? flow.Name : "";
        }

        /// <summary>
        /// 激活临时任务
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="stepID"></param>
        /// <param name="groupID"></param>
        /// <param name="completedTime">要求完成时间</param>
        /// <returns></returns>
        private int UpdateTempTasks(Guid flowID, Guid stepID, Guid groupID, DateTime? completedTime, DateTime receiveTime)
        {
            try
            {
                var query =
                    _workFlowTaskRepository.GetAll()
                        .Where(r => r.FlowID == flowID && r.StepID == stepID && r.GroupID == groupID && r.Status == -1);
                foreach (var workFlowTask in query)
                {
                    workFlowTask.CompletedTime = completedTime;
                    workFlowTask.ReceiveTime = receiveTime;
                    workFlowTask.SenderTime = receiveTime;
                    workFlowTask.Status = 0;
                    _workFlowTaskRepository.Update(workFlowTask);
                }
                return query.Count();
            }
            catch (Exception e)
            {
                return 0;
            }

        }


        /// <summary>
        /// 删除ShortMessage记录
        /// </summary>
        private int DeleteShortMessage(string linkID, int Type)
        {
            return 0;
            //string sql = $"DELETE FROM ShortMessage WHERE LinkID='{linkID}' AND Type={Type}";
            //var ret = _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
            ////new RoadFlow.Platform.ShortMessage().RemoveCache(linkID, Type);
            //return ret;
        }

        /// <summary>
        /// 得到一个连接一个表一个字段的值
        /// </summary>
        /// <param name="link_table_field"></param>
        /// <returns></returns>
        private string GetFieldValueSql(string link_table_field, string pkField, string pkFieldValue)
        {
            if (link_table_field.IsNullOrEmpty())
            {
                return "";
            }
            string[] array = link_table_field.Split('.');
            if (array.Length != 3)
            {
                return "";
            }
            string link = array[0];
            string table = array[1];
            string field = array[2];
            Guid linkid;
            if (!link.IsGuid(out linkid))
            {
                return "";
            }
            string value = string.Empty;
            value = $"SELECT {field} FROM {table} WHERE {pkField} = '{pkFieldValue}'";
            return value;
        }

        private string GetFieldValueSqlWithDefaultMemberModel(string link_table_field, string pkField, string pkFieldValue)
        {
            if (link_table_field.IsNullOrEmpty())
            {
                return "";
            }
            string[] array = link_table_field.Split('.');
            if (array.Length != 3)
            {
                return "";
            }
            string link = array[0];
            string table = array[1];
            string field = array[2];
            Guid linkid;
            if (!link.IsGuid(out linkid))
            {
                return "";
            }
            string value = string.Empty;
            value = $"SELECT NEWID() AS Id, {field} AS [Key]  FROM {table} WHERE {pkField} = '{pkFieldValue}'";
            return value;
        }

        /// <summary>
        /// 判断一个任务是否可以收回
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="isHasten">是否可以崔办</param>
        /// <returns></returns>
        public bool HasWithdraw(Guid taskID, out bool isHasten)
        {
            isHasten = false;
            var taskList = GetNextTaskList(taskID);
            if (taskList.Count == 0) return false;
            foreach (var task in taskList)
            {
                if (task.Status.In(0, 1))
                {
                    isHasten = true;
                    break;
                }
            }
            foreach (var task in taskList)
            {
                ///如果后续有子流程 则不能收回
                if (task.Type == 6)
                    return false;
                if (task.Status != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 收回任务
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool WithdrawTask(Guid taskID)
        {
            var taskList1 = GetTaskList2(taskID);
            if (taskList1 == null || taskList1.Count == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到要收回的待办信息。");
            }
            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            //{
            foreach (var task in taskList1)
            {
                var taskList2 = GetNextTaskList(task.Id);
                foreach (var task2 in taskList2)
                {
                    if (task2.Status.In(-1, 0, 1, 5))
                    {
                        _workFlowTaskRepository.Delete(r => r.Id == task2.Id);
                        //Delete(task2.ID);
                    }
                    if (!task2.SubFlowGroupID.IsNullOrEmpty())
                    {
                        foreach (string groupID in task2.SubFlowGroupID.Split(','))
                        {
                            DeleteInstance(Guid.Empty, groupID.ToGuid());
                        }
                    }
                }

                if (task.Id == taskID || task.Status == 4)
                {
                    Completed(task.Id, "", false, 1, "");
                }
            }
            //  scope.Complete();


            return true;
            //}
        }


        /// <summary>
        /// 判断sql流转条件是否满足
        /// </summary>
        /// <param name="linkID"></param>
        /// <param name="table"></param>
        /// <param name="tablepk"></param>
        /// <param name="instabceID">实例ID</param>
        /// <param name="where"></param>
        /// <returns></returns>

        public bool TestLineSql(Guid linkID, string table, string tablepk, string instabceID, string where)
        {
            if (instabceID.IsNullOrEmpty())
            {
                return false;
            }
            string sql = "SELECT NEWID() AS Id ,COUNT(*) AS Count FROM " + table + " WHERE " + tablepk + "='" + instabceID + "' AND (" + where + ")".ReplaceSelectSql();
            var result = _workFlowTaskRepository.GetSqlConditionResult(sql);
            if (result == null) return false;
            else { return result.Count > 0; }
        }


        public string GetStatusTitle(Guid flowID, int status)
        {
            if (status == -1)
                return "完成";
            else if (status == -2)
                return "终止";
            //else if (status == 0) {
            //    return "草稿";
            //}
            else
            {
                var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
                var stepModel = flow.Steps.FirstOrDefault(r => r.StepToStatus == status);
                if (stepModel != null)
                {
                    return stepModel.StepToStatusTitle;
                }
                else
                {
                    return GetStatusTitleDiGui(flowID, flow.VersionNum - 1, status);
                }

            }
        }

        public string GetStatusTitleDiGui(Guid flowId, int versionNum, int status)
        {
            if (versionNum < 1)
                return "";
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId, versionNum);
            if (flowModel != null)
            {
                var stepModel = flowModel.Steps.FirstOrDefault(r => r.StepToStatus == status);
                if (stepModel != null)
                {
                    return stepModel.StepToStatusTitle;
                }
                else
                {

                    return GetStatusTitleDiGui(flowId, flowModel.VersionNum - 1, status);
                }
            }
            else
            {
                return "";
            }
        }



        public WorkFlowStep GetStepWithStatus(Guid flowID, int status)
        {
            if (status == -1)
                return null;
            else if (status == -2)
                return null;
            //else if (status == 0) {
            //    return "草稿";
            //}
            else
            {
                var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
                var stepModel = flow.Steps.FirstOrDefault(r => r.StepToStatus == status);
                if (stepModel != null)
                {
                    return stepModel;
                }
                else
                {
                    return GetStepWithStatusDiGui(flowID, flow.VersionNum - 1, status);
                }

            }
        }

        public WorkFlowStep GetStepWithStatusDiGui(Guid flowId, int versionNum, int status)
        {
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId, versionNum);
            if (flowModel != null)
            {
                var stepModel = flowModel.Steps.FirstOrDefault(r => r.StepToStatus == status);
                if (stepModel != null)
                {
                    return stepModel;
                }
                else
                {

                    return GetStepWithStatusDiGui(flowId, flowModel.VersionNum - 1, status);
                }
            }
            else
            {
                return null;
            }
        }



        public WorkFlowStep GetStepWithStepId(Guid flowID, Guid stepId)
        {


            var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
            var stepModel = flow.Steps.FirstOrDefault(r => r.ID == stepId);
            if (stepModel != null)
            {
                return stepModel;
            }
            else
            {
                return GetStepWithStepIdDiGui(flowID, flow.VersionNum - 1, stepId);
            }


        }

        public WorkFlowStep GetStepWithStepIdDiGui(Guid flowId, int versionNum, Guid stepId)
        {
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId, versionNum);
            if (flowModel != null)
            {
                var stepModel = flowModel.Steps.FirstOrDefault(r => r.ID == stepId);
                if (stepModel != null)
                {
                    return stepModel;
                }
                else
                {

                    return GetStepWithStepIdDiGui(flowId, flowModel.VersionNum - 1, stepId);
                }
            }
            else
            {
                return null;
            }
        }






        /// <summary>
        /// 更新实例的处理人员
        /// </summary>
        /// <param name="currenttask"></param>
        /// <param name="newTask"></param>
        public void UpdateInstanceDealWithUsers(WorkFlowTask currenttask, WorkFlowTask newTask)
        {

            #region 
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowModel = workFlowCacheManager.GetWorkFlowModelFromCache(currenttask.FlowID);

            if (workflowModel.TitleField != null && workflowModel.TitleField.LinkID != Guid.Empty && !workflowModel.TitleField.Table.IsNullOrEmpty()
                && !workflowModel.TitleField.Field.IsNullOrEmpty() && workflowModel.DataBases.Any())
            {
                var firstDB = workflowModel.DataBases.First();
                try
                {
                    var query_Sql = $"select DealWithUsers from {workflowModel.TitleField.Table} where  {firstDB.PrimaryKey}=\'{currenttask.InstanceID}\'";
                    var instanceDealWithUsers = _dynamicRepository.QueryFirst(query_Sql);
                    var resultDealWithUsers = "";
                    if (instanceDealWithUsers.DealWithUsers == null)
                    {
                        resultDealWithUsers = newTask.ReceiveID.ToString();
                    }
                    else
                    {
                        var exite_UserIdStr = instanceDealWithUsers.DealWithUsers as string;
                        var exite_UserIds = exite_UserIdStr.Split(',');
                        if (exite_UserIds.Contains(newTask.ReceiveID.ToString()))
                        {
                            return;
                        }
                        else
                        {
                            resultDealWithUsers = $"{instanceDealWithUsers.DealWithUsers},{newTask.ReceiveID}";
                        }
                    }
                    string sql = $"UPDATE {workflowModel.TitleField.Table} SET DealWithUsers=\'{resultDealWithUsers}\' WHERE {firstDB.PrimaryKey}=\'{currenttask.InstanceID}\'";
                    _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                }
                catch (Exception err)
                {
                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                    Abp.Logging.LogHelper.Logger.Error($"更新业务处理人员发生了错误({workflowModel.Name}),flowid:{currenttask.FlowID},taskid:{currenttask.Id},errormsg:{err.Message}");
                    throw err;
                }
            }
            #endregion

        }


        /// <summary>
        /// 更新实例的抄送人员
        /// </summary>
        /// <param name="task"></param>
        /// <param name="newTask"></param>
        public void UpdateInstanceCopyForUsers(WorkFlowTask task, List<long> receiveIds)
        {

            #region 
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var workflowModel = workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID, task.VersionNum);
            if (workflowModel.TitleField != null && workflowModel.TitleField.LinkID != Guid.Empty && !workflowModel.TitleField.Table.IsNullOrEmpty()
                && !workflowModel.TitleField.Field.IsNullOrEmpty() && workflowModel.DataBases.Any())
            {
                var firstDB = workflowModel.DataBases.First();
                try
                {
                    var query_Sql = $"select CopyForUsers from {workflowModel.TitleField.Table} where  {firstDB.PrimaryKey}=\'{task.InstanceID}\'";
                    var instanceCopyForUsers = _dynamicRepository.QueryFirst(query_Sql);
                    var resultCopyForUsers = "";
                    if (instanceCopyForUsers.DealWithUsers == null)
                    {
                        resultCopyForUsers = string.Join(',', receiveIds);
                    }
                    else
                    {
                        var exite_UserIdStr = instanceCopyForUsers.CopyForUsers as string;
                        var exite_UserIds = exite_UserIdStr.Split(',');
                        var newusers = receiveIds.Except(exite_UserIds.Select(r => r.ToLong()).ToList());
                        if (newusers.Count() > 0)
                        {
                            resultCopyForUsers = $"{instanceCopyForUsers.CopyForUsers},{string.Join(',', newusers)}";
                        }
                    }
                    string sql = $"UPDATE {workflowModel.TitleField.Table} SET CopyForUsers=\'{resultCopyForUsers}\' WHERE {firstDB.PrimaryKey}=\'{task.InstanceID}\'";
                    _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                }
                catch (Exception err)
                {
                    //var logappService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ILogAppService>();
                    //logappService.CreateOrUpdateLogV2("更新流程完成标题发生了错误", $"Message:{err.Message} StackTrace:{err.StackTrace} model:{executeModel.Serialize()} ", "系统错误");
                    Abp.Logging.LogHelper.Logger.Error($"更新抄送人员发生了错误({workflowModel.Name}),flowid:{task.FlowID},taskid:{task.Id},errormsg:{err.Message}");
                    throw err;
                }
            }
            #endregion

        }




        /// <summary>
        /// 得到两个步骤之间的步骤
        /// </summary>
        /// <param name="runModel"></param>
        /// <param name="stepID1"></param>
        /// <param name="stepID2"></param>
        /// <returns></returns>
        public List<WorkFlowStep> getBetweenSteps(WorkFlowInstalled runModel, Guid stepID1, Guid stepID2)
        {
            var steps = new List<WorkFlowStep>();
            addBetweenSteps(steps, runModel, stepID1, stepID2);
            return steps;
        }
        private void addBetweenSteps(List<WorkFlowStep> steps, WorkFlowInstalled runModel, Guid stepID1, Guid stepID2)
        {
            var flowSteps = runModel.Steps.ToList();
            foreach (var line in runModel.Lines)
            {
                if (line.ToID == stepID2)
                {
                    var step = flowSteps.Find(p => p.ID == line.FromID);
                    if (step != null && step.ID != stepID1)
                    {
                        steps.Add(step);
                        addBetweenSteps(steps, runModel, stepID1, step.ID);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }


        #endregion


    }
}
