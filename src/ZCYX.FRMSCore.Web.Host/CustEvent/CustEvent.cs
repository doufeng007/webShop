using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.WorkFlow;
using Castle.Windsor;
using HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    public class CustEvent
    {
        //private static IProjectAppService _projectAppService;

        //public CustEvent(IProjectAppService projectAppService)
        //{
        //    _projectAppService = projectAppService;
        //}

        /// <summary>
        /// 出差关联用户权限转移
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool WorkoutRelationUserId(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAWorkoutAppService>();
            _projectAppService.WorkoutRelationUserId(eventParams.InstanceID.ToGuid());
            return true;
        }

        /// <summary>
        /// 请假关联用户权限转移
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool AskForLeaveRelationUserId(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IEmployeeAskForLeaveAppService>();
            _projectAppService.AskForLeaveRelationUserId(eventParams.InstanceID.ToGuid());
            return true;
        }
   

   

  



      

       
    


        /// <summary>
        /// 当前任务处理者是否是部门领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CurrentRecevieUserIsChargeLeader(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var taskModel = repository.Get(eventParams.TaskID);
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            return manager.IsChargerLeader(taskModel.ReceiveID);
        }

        /// <summary>
        /// 当前任务处理者是否是部门领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CurrentRecevieUserIsNotChargeLeader(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var taskModel = repository.Get(eventParams.TaskID);
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            return !manager.IsChargerLeader(taskModel.ReceiveID);
        }

      
     




    }
}
