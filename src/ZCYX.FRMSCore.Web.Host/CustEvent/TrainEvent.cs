using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Train;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    public class TrainEvent
    {
        private readonly ICourseAppService _courseRecommendAppService;
        public TrainEvent()
        {
             _courseRecommendAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICourseAppService>();
        }
        /// <summary>
        /// 课程推荐成功后给推荐人发消息
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public void SendMessageForRecommendUser(WorkFlowCustomEventParams eventParams)
        {
           _courseRecommendAppService.SendMessageForRecommendUser(eventParams);
        }
        /// <summary>
        /// 获取需要填写心得的人
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetNeedExperienceUsers(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Train.ITrainAppService>();
            return service.GetNeedExperienceUsers(eventParams.InstanceID);
        }
        /// <summary>
        /// 总经理审批后提醒
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public void SendMessageByUser(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Train.ITrainAppService>();
             service.SendMessageByUser(eventParams.InstanceID.ToGuid());
        }

        /// <summary>
        /// 获取需要接受抄送的人
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetTrainNeedCopyForUsers(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Train.ITrainAppService>();
            return service.GetTrainNeedCopyForUsers(eventParams.InstanceID);
        }
        /// <summary>
        /// 获取所有参会人员的leader
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetExperienceLeader(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Train.ITrainAppService>();
            return service.GetExperienceLeader(eventParams.InstanceID);
        }
        /// <summary>
        /// 增加参与积分
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public void AddJoinScore(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Train.ITrainAppService>();
             service.AddJoinScore(eventParams.InstanceID);
        }


        /// <summary>
        /// 激活请假子流程
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public WorkFlowExecute TrainLeaveFlowActive(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITrainLeaveAppService>();
            var data = _service.TrainLeaveFlowActive(eventParams.InstanceID.ToGuid());
            var ret = new WorkFlowExecute();
            ret.Title = $"培训-请假";
            ret.InstanceID = data;
            return ret;
        }
        /// <summary>
        /// 提交请假前验证
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool TrainLeaveVerification(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITrainLeaveAppService>();
            return _service.TrainLeaveVerification(eventParams.InstanceID.ToGuid());

        }
        /// <summary>
        /// 提交请假前验证
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetTrainLeaveAuditUser(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ITrainLeaveAppService>();
            return _service.GetTrainLeaveAuditUser(eventParams.InstanceID.ToGuid());

        }

        /// <summary>
        /// 获取课程心得体会人员的部门领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetCouserExperienceUserLeader(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IUserCourseExperienceAppService>();
            return service.GetCouserExperienceUserLeader(eventParams.InstanceID);
        }
    }
}
