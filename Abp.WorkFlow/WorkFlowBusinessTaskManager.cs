using Abp.Application.Services;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Reflection.Extensions;
using Abp.UI;
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
using ZCYX.FRMSCore.Model;

namespace Abp.WorkFlow
{
    [RemoteService(IsEnabled = false)]
    public class WorkFlowBusinessTaskManager : ApplicationService
    {
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<User, long> _useRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;

        public WorkFlowBusinessTaskManager(IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowCacheManager workFlowCacheManager, IRepository<User, long> useRepository
            , IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , WorkFlowTaskManager workFlowTaskManager)
        {
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _useRepository = useRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }
  
        ///// <summary>
        ///// 补充流程业务数据
        ///// </summary>
        ///// <param name="input"></param>
        //[AbpAuthorize]
        //public List<BusinessWorkFlowListOutput> SupplementWorkFlowBusinessList(Guid flowId, List<BusinessWorkFlowListOutput> input)
        //{
        //    //var query = from task in _workFlowTaskRepository.GetAll()
        //    //            join business in input on task.InstanceID equals business.Id.ToString()
        //    //            join task_currenUser in _workFlowTaskRepository.GetAll() on new { taskId = business.Id, userId = AbpSession.UserId.Value }
        //    //            equals new { taskId = task_currenUser.Id, userId = task_currenUser.ReceiveID } into g
        //    var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
        //    foreach (var item in input)
        //    {
        //        var stepModel = wfInstalled.Steps.FirstOrDefault(r => r.StepToStatus == item.Status);
        //        item.StatusTitle = stepModel.StepToStatusTitle;
        //        var task = _workFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == item.InstanceId.ToString() && r.ReceiveID == AbpSession.UserId.Value
        //       && r.StepID == stepModel.ID);
        //        if (task.Count() == 0)
        //            item.OpenModel = 2;
        //        else
        //        {
        //            var waitDoTask = task.FirstOrDefault(r => r.Status == -1 || r.Status == 0);
        //            if (waitDoTask != null)
        //            {
        //                item.OpenModel = 1;
        //                item.TaskId = waitDoTask.Id;
        //                item.GroupId = waitDoTask.GroupID;
        //                item.StepId = stepModel.ID;
        //            }
        //            else
        //            {
        //                item.OpenModel = 2;
        //            }
        //        }

        //    }
        //    return input;

        //}

        /// <summary>
        /// 补充流程业务数据  在GetList里面已经对openModel 进行处理， 这里不需要再对openmodel处理
        /// </summary>
        /// <param name="input"></param>
        [AbpAuthorize]
        public void SupplementWorkFlowBusinessList(Guid flowId, BusinessWorkFlowListOutput input)
        {
            if (input.InstanceId == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未指定instanceId");
            var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            var firstStep = wfInstalled.Steps.FirstOrDefault();
            if (input.Status == (int)WorkFlowBusinessStatus.完成)
            {
                input.StatusTitle = "完成";
                input.CurrentStepNames = "完成";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.驳回)
            {
                input.StatusTitle = "驳回";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "驳回";
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.整改中)
            {
                input.StatusTitle = "整改中";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "整改中";
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.已终止)
            {
                input.StatusTitle = "已终止";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "已终止";
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.作废)
            {
                input.StatusTitle = "作废";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "作废";
            }
            else if (input.Status >= 0)
            {
                //hq:子查询隐藏待办无法找到步骤状态名称补丁
                var step = wfInstalled.Steps.FirstOrDefault(x => x.StepToStatus == input.Status);
                input.StatusTitle = step?.StepToStatusTitle;

                var staues = new List<int>();
                staues.Add(0);
                staues.Add(1);
                //档案借阅    用品维修   用品报废 
                if (string.Compare(flowId.ToString(), "81427e46-80cc-4306-b931-b57973a699c8", true) == 0 ||
                    string.Compare(flowId.ToString(), "703A5047-017B-41F7-A558-D0AFAAD7AC32", true) == 0 ||
                    string.Compare(flowId.ToString(), "665548AE-A5FB-4EEE-89DE-A00B6875949E", true) == 0)
                {
                    staues.Add(-1);
                }
                var currentTasks = _workFlowTaskRepository.GetAll().Where(r => r.InstanceID == input.InstanceId && r.Type != 6 && r.FlowID == flowId && staues.Contains(r.Status));
                if (currentTasks.Count() > 0)
                {
                    var currentStatusTitles = new List<string>();
                    foreach (var item in currentTasks.OrderBy(r => r.Sort))
                    {
                        //抄送待办不参与当前步骤展示 zcl
                        if (item.Type == 5)
                            continue;
                        var stepModel = _workFlowTaskManager.GetStepWithStepId(flowId, item.StepID);
                        currentStatusTitles.Add(stepModel.StepToStatusTitle);

                    }
                    currentStatusTitles = currentStatusTitles.Distinct().ToList();
                    input.CurrentStepNames = string.Join(",", currentStatusTitles);
                    input.StatusTitle = input.CurrentStepNames;

                    var waitDoTasks = currentTasks.Where(r => r.ReceiveID == AbpSession.UserId.Value);
                    if (waitDoTasks.Count() > 0)
                    {
                        input.OpenModel = 1;
                        if (waitDoTasks.Count() > 1)
                        {
                            input.IsMultipleStep = true;
                            foreach (var item in waitDoTasks)
                            {
                                var entity = new TodoParameter()
                                {
                                    GroupId = item.GroupID,
                                    StepId = item.StepID,
                                    TaskId = item.Id,

                                };
                                var stepModel = _workFlowTaskManager.GetStepWithStepId(flowId, item.StepID);
                                entity.WorkFlowModelId = stepModel.WorkFlowModelId.Value;
                                input.DoParameters.Add(entity);

                            }
                        }
                        else
                        {
                            var waitDoTask = waitDoTasks.First();
                            input.TaskId = waitDoTask.Id;
                            input.GroupId = waitDoTask.GroupID;
                            input.StepId = waitDoTask.StepID;
                            var stepModel = _workFlowTaskManager.GetStepWithStepId(flowId, waitDoTask.StepID);
                            input.WorkFlowModelId = stepModel.WorkFlowModelId;
                        }

                    }
                    else
                    {
                        input.OpenModel = 2;
                        input.WorkFlowModelId = firstStep.WorkFlowModelId;
                    }

                }
                else
                {
                    if (step != null)
                    {
                        //input.WorkFlowModelId = step.WorkFlowModelId;
                        input.WorkFlowModelId = firstStep.WorkFlowModelId;   //不是当前处理者 看第一步模板
                    }
                    else
                    {
                        input.WorkFlowModelId = firstStep.WorkFlowModelId;
                    }
                    input.OpenModel = 2;
                    input.CurrentStepNames = input.StatusTitle;
                }
            }
            else
            {
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "";
            }






        }


        public void SupplementWorkFlowBusinessListForSupplyScrap(Guid flowId, BusinessWorkFlowListOutput input, out FirstTaskModelScrap firstTaskModel)
        {
            if (input.InstanceId == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未指定instanceId");
            var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            var firstStep = wfInstalled.Steps.FirstOrDefault();
            if (input.Status == (int)WorkFlowBusinessStatus.完成)
            {
                input.StatusTitle = "完成";
                input.CurrentStepNames = "完成";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.驳回)
            {
                input.StatusTitle = "驳回";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "驳回";
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.整改中)
            {
                input.StatusTitle = "整改中";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "整改中";
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.已终止)
            {
                input.StatusTitle = "已终止";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "已终止";
            }
            else if (input.Status == (int)WorkFlowBusinessStatus.作废)
            {
                input.StatusTitle = "作废";
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "作废";
            }
            else if (input.Status >= 0)
            {
                //hq:子查询隐藏待办无法找到步骤状态名称补丁
                var step = wfInstalled.Steps.FirstOrDefault(x => x.StepToStatus == input.Status);
                input.StatusTitle = step?.StepToStatusTitle;

                var staues = new List<int>();
                staues.Add(0);
                staues.Add(1);
                //档案借阅    用品维修   用品报废 
                if (string.Compare(flowId.ToString(), "81427e46-80cc-4306-b931-b57973a699c8", true) == 0 ||
                    string.Compare(flowId.ToString(), "703A5047-017B-41F7-A558-D0AFAAD7AC32", true) == 0 ||
                    string.Compare(flowId.ToString(), "665548AE-A5FB-4EEE-89DE-A00B6875949E", true) == 0)
                {
                    staues.Add(-1);
                }
                var currentTasks = _workFlowTaskRepository.GetAll().Where(r => r.InstanceID == input.InstanceId && r.Type != 6 && r.FlowID == flowId && staues.Contains(r.Status));
                if (currentTasks.Count() > 0)
                {
                    var currentStatusTitles = new List<string>();
                    foreach (var item in currentTasks.OrderBy(r => r.Sort))
                    {
                        //抄送待办不参与当前步骤展示 zcl
                        if (item.Type == 5)
                            continue;
                        var stepModel = _workFlowTaskManager.GetStepWithStepId(flowId, item.StepID);
                        currentStatusTitles.Add(stepModel.StepToStatusTitle);

                    }
                    currentStatusTitles = currentStatusTitles.Distinct().ToList();
                    input.CurrentStepNames = string.Join(",", currentStatusTitles);
                    input.StatusTitle = input.CurrentStepNames;

                    var waitDoTasks = currentTasks.Where(r => r.ReceiveID == AbpSession.UserId.Value);
                    if (waitDoTasks.Count() > 0)
                    {
                        input.OpenModel = 1;
                        if (waitDoTasks.Count() > 1)
                        {
                            input.IsMultipleStep = true;
                            foreach (var item in waitDoTasks)
                            {
                                var entity = new TodoParameter()
                                {
                                    GroupId = item.GroupID,
                                    StepId = item.StepID,
                                    TaskId = item.Id,

                                };
                                var stepModel = _workFlowTaskManager.GetStepWithStepId(flowId, item.StepID);
                                entity.WorkFlowModelId = stepModel.WorkFlowModelId.Value;
                                input.DoParameters.Add(entity);

                            }
                        }
                        else
                        {
                            var waitDoTask = waitDoTasks.First();
                            input.TaskId = waitDoTask.Id;
                            input.GroupId = waitDoTask.GroupID;
                            input.StepId = waitDoTask.StepID;
                            var stepModel = _workFlowTaskManager.GetStepWithStepId(flowId, waitDoTask.StepID);
                            input.WorkFlowModelId = stepModel.WorkFlowModelId;
                        }

                    }
                    else
                    {
                        input.OpenModel = 2;
                        input.WorkFlowModelId = firstStep.WorkFlowModelId;
                    }

                }
                else
                {
                    if (step != null)
                    {
                        //input.WorkFlowModelId = step.WorkFlowModelId;
                        input.WorkFlowModelId = firstStep.WorkFlowModelId;   //不是当前处理者 看第一步模板
                    }
                    else
                    {
                        input.WorkFlowModelId = firstStep.WorkFlowModelId;
                    }
                    input.OpenModel = 2;
                    input.CurrentStepNames = input.StatusTitle;
                }
            }
            else
            {
                input.WorkFlowModelId = firstStep.WorkFlowModelId;
                input.OpenModel = 2;
                input.CurrentStepNames = "";
            }

            firstTaskModel = new FirstTaskModelScrap();
            var firstTask = _workFlowTaskRepository.FirstOrDefault(r => r.FlowID == flowId && r.StepID == firstStep.ID);
            firstTaskModel.GroupId = firstTask.GroupID;
            firstTaskModel.StepId = firstTask.StepID;
            firstTaskModel.TaskId = firstTask.Id;


        }
    }
}
