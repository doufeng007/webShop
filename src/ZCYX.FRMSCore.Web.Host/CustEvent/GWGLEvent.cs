using Abp.WorkFlow;
using CWGL;
using GWGL;
using Project;

namespace ZCYX.FRMSCore.Web.Host
{
    /// <summary>
    /// 财务流程自定义事件
    /// </summary>
    public class GWGLEvent
    {
        public bool IsEmployeeAndNeedAddWrite(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            var flag1 = _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 4);
            var _service2 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<INoticeDocumentAppService>();
            var flag2 = _service2.IsNeedAddWrite(new System.Guid(eventParams.InstanceID));
            return flag1 && flag2;
        }
        public bool IsBMLDAndNeedAddWrite(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            var flag1 = _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 3);
            var _service2 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<INoticeDocumentAppService>();
            var flag2 = _service2.IsNeedAddWrite(new System.Guid(eventParams.InstanceID));
            return flag1 && flag2;
        }


        public bool IsNotNeedAddWrite(WorkFlowCustomEventParams eventParams)
        {
            var _service2 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<INoticeDocumentAppService>();
            var flag2 = _service2.IsNeedAddWrite(new System.Guid(eventParams.InstanceID));
            return !flag2;
        }



        public void MarkCheckUser(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<INoticeDocumentAppService>();
            _service.MarkCheckUser(eventParams.InstanceID);
        }

        //public void AudtAddWriteGWFW_FGLDAudit(WorkFlowCustomEventParams eventParams)
        //{
        //    var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<INoticeDocumentAppService>();
        //    if (_service.IsNeedAddWrite(new System.Guid(eventParams.InstanceID)))
        //        _service.AudtAddWrite(eventParams.InstanceID, eventParams.TaskID);
        //}
    }
}
