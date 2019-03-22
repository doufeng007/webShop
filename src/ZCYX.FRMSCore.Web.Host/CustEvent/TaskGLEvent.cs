using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using CWGL;
using MeetingGL;
using TaskGL;
using XZGL;
using ZCYX;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    /// <summary>
    /// 任务自定义事件
    /// </summary>
    public class TaskGLEvent
    {
        public void Reject(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITaskManagementAppService>();
            service.UpdateTaskStatus(eventParams.InstanceID.ToGuid(), TaskManagementStateEnum.Reject);
        }

        public void Complete(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITaskManagementAppService>();
            service.UpdateTaskStatus(eventParams.InstanceID.ToGuid(), TaskManagementStateEnum.BefreStart);
        }

        public void CreateComplete(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITaskManagementAppService>();
            service.CreateComplete(eventParams.InstanceID.ToGuid());

        }

        /// <summary>
        /// 查找部门领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string FindAuditUserDepartmentLeader(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITaskManagementAppService>();
            return service.FindAuditUser(eventParams.InstanceID.ToGuid(), 1);
        }

        /// <summary>
        /// 查找部门分管领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string FindAuditUserOrgFGLD(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITaskManagementAppService>();
            return service.FindAuditUser(eventParams.InstanceID.ToGuid(), 2);
        }


        /// <summary>
        /// 查找总经理
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string FindAuditUserZJL(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITaskManagementAppService>();
            return service.FindAuditUser(eventParams.InstanceID.ToGuid(), 3);
        }
    }
}
