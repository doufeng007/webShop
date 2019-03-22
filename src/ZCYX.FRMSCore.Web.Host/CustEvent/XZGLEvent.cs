using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using CWGL;
using MeetingGL;
using XZGL;
using ZCYX;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    public class XZGLEvent
    {
        #region 会议流程相关

        public bool IsZJLUserCar(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            var Isszjl= _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 1, "UserId");
            if (!Isszjl)
                return false;
            var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLCarBorrowAppService>();
            var model = _service1.GetCarBorrow(eventParams.InstanceID);
            if ((model.CarType == CarType.个人用车 || model.CarType ==CarType.私车公用) && !model.IsCompanyCar)
                return true;
            return false;
        }

        public bool IsZJLCompanyCar(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            var Isszjl= _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 1, "UserId");
            if (!Isszjl)
                return false;
            var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLCarBorrowAppService>();
            var model = _service1.GetCarBorrow(eventParams.InstanceID);
            if (model.IsCompanyCar)
                return true;
            return false;
        }


        public string GetCarDriver(WorkFlowCustomEventParams eventParams)
        {
            var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLCarBorrowAppService>();
            return _service1.GetCarDriver(eventParams.InstanceID);
        }


        public bool IsZJLCompanyRentCar(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            var Isszjl= _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 1, "UserId");
            if (!Isszjl)
                return false;
            var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLCarBorrowAppService>();
            var model = _service1.GetCarBorrow(eventParams.InstanceID);
            if (!model.IsCompanyCar && model.IsCompanyRent)
                return true;
            return false;
        }
        public bool IsZJL(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 1, "UserId");
        }
        public string CopyForZJL(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            var iszjl= _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 1, "UserId");
            if (iszjl)
            {
                var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLCarBorrowAppService>();
                return _service1.GetUserId(eventParams.InstanceID);
            }
            return "";
        }
        public string CopyForCarBorrowUser(WorkFlowCustomEventParams eventParams)
        {
            var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLCarBorrowAppService>();
            return _service1.GetUserId(eventParams.InstanceID);
        }

        public bool IsFGLD(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 2, "UserId");
        }
        public bool IsBMLD(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 3, "UserId");
        }
        public bool IsPTYG(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLBorrowMoneyAppService>();
            return _service.IsRole(eventParams.FlowID, eventParams.GroupID, eventParams.InstanceID, 4, "UserId");
        }
        /// <summary>
        /// 是否是行政人员
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CheckPostFromClerical(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IXZGLMeetingAppService>();
            return _service.CheckPostFromUser(eventParams.TaskID, 0);
        }

        /// <summary>
        /// 是否是分管领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CheckPostFromSection(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IXZGLMeetingAppService>();
            return _service.CheckPostFromUser(eventParams.TaskID, 1);
        }

        /// <summary>
        /// 是否是最高领导&行政
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CheckPostFromClericalAndBoss(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IXZGLMeetingAppService>();
            return _service.CheckPostFromUser(eventParams.TaskID, 2) || _service.CheckPostFromUser(eventParams.TaskID, 0);
        }
        /// <summary>
        /// 抄送会议
        /// </summary>
        /// <param name="eventParams"></param>
        public void NotifyMeeting(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IXZGLMeetingAppService>();
            _service.NotifyMeeting(eventParams);
        }
        #endregion


    }
}
