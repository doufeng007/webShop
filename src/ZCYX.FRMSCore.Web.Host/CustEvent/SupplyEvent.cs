using Abp.Domain.Repositories;
using Abp.WorkFlow;
using Microsoft.AspNetCore.Identity;
using Supply;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XZGL;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Users;

namespace ZCYX.FRMSCore.Web.Host
{
    public class SupplyEvent
    {
        /// <summary>
        /// 用品退还申请驳回后，重置用品的状态为使用中
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool ReSetBackSupplyStatus(WorkFlowCustomEventParams eventParams)
        {


            var subrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<SupplyBackSub, Guid>>();
            var sub = subrepository.GetAll().Where(ite => ite.MainId == Guid.Parse(eventParams.InstanceID)).ToList();
            var usersupplyrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<UserSupply, Guid>>();
            var supplyids = sub.Select(ite => ite.SupplyId).ToList();
            var usersupplys = usersupplyrepository.GetAll().Where(ite => supplyids.Contains(ite.SupplyId) && ite.Status == (int)UserSupplyStatus.退还中).ToList();
            foreach (var item in usersupplys)
            {
                item.Status = (int)UserSupplyStatus.使用中;
                usersupplyrepository.Update(item);
            }
            var supplyrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<SupplyBase, Guid>>();
            var supplys = supplyrepository.GetAll().Where(ite => supplyids.Contains(ite.Id) && ite.Status == (int)SupplyStatus.退还中).ToList();
            foreach (var item in supplys)
            {
                item.Status = (int)SupplyStatus.被领用;
                supplyrepository.Update(item);
            }
            return true;
        }
        /// <summary>
        /// 用品维修申请驳回状态处理
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool ReSetRepairSupplyStatus(WorkFlowCustomEventParams eventParams)
        {


            var repairrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<SupplyRepair, Guid>>();
            var repair = repairrepository.GetAll().First(ite => ite.Id == Guid.Parse(eventParams.InstanceID));

            var usersupplyrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<UserSupply, Guid>>();

            var usersupplys = usersupplyrepository.FirstOrDefault(ite => ite.SupplyId == repair.SupplyId && ite.Status == (int)UserSupplyStatus.维修中);
            if (usersupplys != null)
            {
                usersupplys.Status = (int)UserSupplyStatus.使用中;
            }
            var supplyrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<SupplyBase, Guid>>();
            var supplys = supplyrepository.Get(repair.SupplyId);
            supplys.Status = (int)SupplyStatus.被领用;
            return true;
        }
        /// <summary>
        /// 用品报废驳回和收回，重置状态为使用中
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool ReSetBackScrapStatus(WorkFlowCustomEventParams eventParams)
        {


            var subrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<SupplyScrapSub, Guid>>();
            var sub = subrepository.GetAll().Where(ite => ite.MainId == Guid.Parse(eventParams.InstanceID)).ToList();
            var usersupplyrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<UserSupply, Guid>>();
            var supplyids = sub.Select(ite => ite.SupplyId).ToList();
            var usersupplys = usersupplyrepository.GetAll().Where(ite => supplyids.Contains(ite.SupplyId) && ite.Status == (int)UserSupplyStatus.报废中).ToList();
            foreach (var item in usersupplys)
            {
                item.Status = (int)UserSupplyStatus.使用中;
                usersupplyrepository.Update(item);
            }
            var supplyrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<SupplyBase, Guid>>();
            var supplys = supplyrepository.GetAll().Where(ite => supplyids.Contains(ite.Id) && ite.Status == (int)SupplyStatus.报废中).ToList();
            foreach (var item in supplys)
            {
                item.Status = (int)SupplyStatus.被领用;
                supplyrepository.Update(item);
            }
            return true;
        }
        /// <summary>
        /// 普通员工发起的任务交给行政处理  行政发起的任务交给总经理审核
        /// </summary>
        /// <returns></returns>
        //public string GetHandler(WorkFlowCustomEventParams eventParams) {
        //    //1.判断发送者是否是行政
        //    //2.获取行政或总经理人员id
        //    var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
        //        .Resolve<IXZGLMeetingAppService>();
        //    var isxzry= _service.CheckPostFromUser(eventParams.TaskID, 0);
        //    var usermanager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IUserAppService>();
        //    if (isxzry)
        //    {
        //        //找总经理角色的人员
        //        var suser = usermanager.GetUserByRoleCode("ZJL").Result;

        //    }
        //    else {
        //        //找行政人员
        //        var suser = usermanager.GetUserByRoleCode("XZRY").Result;
        //    }
        //}



        /// <summary>
        /// 采购是否入库完
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public ExecuteWorkFlowOutput IsComplatePutin(WorkFlowCustomEventParams eventParams)
        {

            var serive = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Supply.Service.SupplyApply.ISupplyApplyAppService>();
            var ret = serive.IsComplatePutin(eventParams.InstanceID);
            return ret;
        }


        /// <summary>
        /// 采购计划-领导审核是否完成
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public ExecuteWorkFlowOutput IsComplateAuditByZJL(WorkFlowCustomEventParams eventParams)
        {

            var serive = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Supply.Service.SupplyApply.ISupplyApplyAppService>();
            var ret = serive.IsComplateAuditByZJL(eventParams.InstanceID);
            return ret;
        }
    }
}
