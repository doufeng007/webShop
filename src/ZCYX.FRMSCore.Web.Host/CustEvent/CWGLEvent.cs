using Abp.WorkFlow;
using CWGL;

namespace ZCYX.FRMSCore.Web.Host
{
    /// <summary>
    /// 财务流程自定义事件
    /// </summary>
    public class CWGLEvent
    {
        public bool IsNoNeedCWCLAudit(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsNeedCWCLAudit(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID);
        }
        public bool IsNeedCWCLAudit(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return !_service.IsNeedCWCLAudit(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID);
        }
        public bool IsZJL(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID,1);
        }
        public bool IsFGLD(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID,2);
        }
        public bool IsBMLD(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID,3);
        }
        public bool IsPTYG(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID,4);
        }
        public bool IsNoCommonLoan(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return !_service.IsCommonLoan(eventParams.FlowID, eventParams.InstanceID);
        }
        public bool IsCommonLoan(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsCommonLoan(eventParams.FlowID, eventParams.InstanceID);
        }
        public void UpdateBorrowMoneyIsPayBack(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            _service.UpdateIsPayBack(eventParams.FlowID, eventParams.InstanceID, false);
        }
        public void SendToZjl(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLReceivableAppService>();
            _service.SendToZjlAsync(eventParams.FlowID, eventParams.InstanceID);
        }

        public bool IsNoNeedCWCLAuditCLF(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLTravelReimbursementAppService>();
            return _service.IsNeedCWCLAuditCLF(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID);
        }
        public bool IsNeedCWCLAuditCLF(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLTravelReimbursementAppService>();
            return !_service.IsNeedCWCLAuditCLF(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID);
        }
        public void CreatePayMoneyCredential(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLPayMoneyAppService>();
            _service.CreateCredential(eventParams.FlowID, eventParams.InstanceID);
        }
        public void UpdateReimbursementIsPayBack(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLReimbursementAppService>();
            _service.UpdateIsPayBack(eventParams.FlowID, eventParams.InstanceID);
        }

        /// <summary>
        /// 差旅费报销后，改变相应借款记录为 已还款
        /// </summary>
        /// <param name="eventParams"></param>
        public void ChangeCWGLBorrowMoneyReturn(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLTravelReimbursementAppService>();
            _service.ChangeCWGLBorrowMoneyReturn(eventParams.InstanceID);
        }

    }
}
