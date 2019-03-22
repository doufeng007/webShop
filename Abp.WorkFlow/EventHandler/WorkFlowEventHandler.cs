using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Model;

namespace Abp.WorkFlow
{
    public class WorkFlowEventHandler : IEventHandler<WorkFlowAutoCompleteEventData>, ISingletonDependency
    {

        public WorkFlowEventHandler()
        {

        }
        public void HandleEvent(WorkFlowAutoCompleteEventData data)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowTaskRepository>();
            var taskModel = repository.Get(data.TaskId);

            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowWorkTaskAppService>();
            var ret = service.ExecuteTaskSync(new ExecuteWorkFlowInput()
            {
                ActionType = "completed",
                Comment = "自动完成",
                FlowId = taskModel.FlowID,
                GroupId = taskModel.GroupID,
                InstanceId = taskModel.InstanceID,
                StepId = taskModel.StepID,
                TaskId = taskModel.Id,
                Title = "自动完成",
            });
            if(!ret.IsSuccesefull)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "自动完成步骤执行失败");

        }
    }
}
