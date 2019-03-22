using Abp.WorkFlow;
using HR;
using Supply;
using Supply.Service;
using Supply.Service.SupplyApply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    /// <summary>
    /// 用品流程自定义事件
    /// </summary>
    public class YPCustEvent
    {

        /// <summary>
        /// 申领处理结果是否包含申购
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool HasSupplyApplyPurchase(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            return _service.HasSupplyApplyPurchase(eventParams.InstanceID.ToGuid());
        }

        /// <summary>
        /// 采购子流程激活自定义事件
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public WorkFlowExecute SupplyApplyPurchaseActive(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            var data = _service.CreateSupplyPurchaseMainForSubFlow(eventParams.InstanceID.ToGuid());
            var ret = new WorkFlowExecute();
            ret.Title = $"{data}申购计划";
            ret.InstanceID = data;
            return ret;
        }


        /// <summary>
        /// 采购完成后自定义事件
        /// </summary>
        /// <param name="instanceId"></param>
        public void ChangeApplyStatusAfterPurchase(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            _service.ChangeApplyStatusAfterPurchase(eventParams.InstanceID);
        }

        public bool IsNeedFGLDAudit(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyAppService>();
            return _service.IsNeedFGLDAudit(eventParams.FlowID, eventParams.GroupID);
        }

        public bool IsNoNeedFGLDAudit(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyAppService>();
            return !_service.IsNeedFGLDAudit(eventParams.FlowID, eventParams.GroupID);
        }


        /// <summary>
        /// 编制计划通过后，更新编制
        /// </summary>
        /// <param name="eventParams"></param>
        public void UpdateBianzhi(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOrganizationUnitPostPlanAppService>();
            _service.CompletaPlan(eventParams.InstanceID);
        }

        /// <summary>
        /// 整改编制的完成事件
        /// </summary>
        /// <param name="instanceId"></param>
        public void CompletaChangePlan(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOrganizationUnitPostPlanAppService>();
            _service.CompletaChangePlan(eventParams.InstanceID);
        }

        /// <summary>
        /// 申领流程中 把处理为申购的数据 加入 申购清单
        /// </summary>
        /// <param name="instacneId"></param>
        /// <returns></returns>
        public void CreateSupplyPurchaseQD(WorkFlowCustomEventParams eventParams)
        {
            var instacneId = eventParams.InstanceID.ToGuid();
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            _service.CreateSupplyPurchaseQD(instacneId);
        }


        /// <summary>
        /// 采购是否存在申领
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsSupplyPruchaseExitApply(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            return _service.IsSupplyPruchaseExitApply(eventParams.InstanceID);
        }


        /// <summary>
        /// 获取采购里的用品申领人员
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string FindSupplyPruchaseRecipientsUsers(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            return _service.FindSupplyPruchaseRecipientsUsers(eventParams.InstanceID);
        }

        /// <summary>
        /// 固化采购调整完成后修改原数据
        /// </summary>
        /// <param name="eventParams"></param>
        public void CompleteEdit(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICuringProcurementEditAppService>();
            _service.CompleteEdit(eventParams.InstanceID.ToGuid());
        }


        /// <summary>
        /// 更新固化采购到整改
        /// </summary>
        /// <param name="eventParams"></param>
        public void UpdateCuringProcurementToChange(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICuringProcurementAppService>();
            _service.UpdateToChange(eventParams.InstanceID.ToGuid());
        }

        /// <summary>
        /// 申领是否处理完成
        /// </summary>
        /// <returns></returns>
        public bool IsComplateApply(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyApplyAppService>();
            return _service.IsComplateApply(eventParams.InstanceID.ToGuid());
        }



        public string GetCreateUserId(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            return service.GetInstanceCreatUserId(eventParams.FlowID, eventParams.InstanceID, eventParams.TaskID.ToString());
        }


        public WorkFlowExecute CreateSubFlowSupplyScrapInstance(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyScrapAppService>();
            var instaceId = service.CreateSubFlowSupplyScrapInstance(eventParams.InstanceID);
            var ret = new WorkFlowExecute();
            ret.Title = $"报废审批";
            ret.InstanceID = instaceId;
            return ret;
        }


        /// <summary>
        /// 报修通知维修商
        /// </summary>
        /// <param name="eventParams"></param>
        public void RepairNoticeSupplierAction(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyRepairAppService>();
            service.RepairNoticeSupplierAction(eventParams.InstanceID, eventParams.OtherString);
        }



        public void AfterRepairAction(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyRepairAppService>();
            service.AfterRepairAction(eventParams.InstanceID, eventParams.FlowID);
        }


        public void AfterScrapAction(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyScrapAppService>();
            service.AfterScrapAction(eventParams.InstanceID, eventParams.FlowID);
        }
    }
}
