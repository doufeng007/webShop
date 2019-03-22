using Abp.WorkFlow;
using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    public class MQEvent
    {
        /// <summary>
        /// 分派给协作单位评审项目---rabbitMQ push消息
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string PushProjectToXieZuo(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectRabbitMQAppService>();
            service.PushProjectToXieZou(eventParams.InstanceID.ToGuid());
            return "成功";
        }
    }
}
