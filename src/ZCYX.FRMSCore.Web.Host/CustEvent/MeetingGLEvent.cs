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
    public class MeetingGLEvent
    {
        #region 会议流程相关
        /// <summary>
        /// 创建会前任务
        /// </summary>
        /// <param name="eventParams"></param>
        public void CreateMeetingBeforeTask(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IXZGLMeetingAppService>();
            _service.CreateMeetingBeforeTask(eventParams.InstanceID.ToGuid());
        }

        /// <summary>
        /// 获取会议确认者
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetMeetingConfirmUser(WorkFlowCustomEventParams eventParams)
        {
            var _repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IRepository<XZGLMeeting, Guid>>();
            var model = _repository.Get(eventParams.InstanceID.ToGuid());
            return $"u_{model.CreatorUserId.Value}";
        }


        /// <summary>
        /// 获取会议记录人
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetMeetingRecodeUser(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IXZGLMeetingAppService>();
            return _service.GetMeetingRecodeUser(eventParams.InstanceID.ToGuid());
        }

        /// <summary>
        /// 获取会议回执请假审核者
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetMeetingCreateUserForReturnReceipt(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IMeetingUserAppService>();
            return _service.GetMeetingCreateUserForReturnReceipt(eventParams.InstanceID.ToGuid());
        }


        /// <summary>
        /// 获取会议发起者
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetMeetingCreateUser(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IMeetingUserAppService>();
            return _service.GetMeetingCreateUser(eventParams.InstanceID.ToGuid());
        }



        public void UpdateNesIssueSatatus(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                   .Resolve<IMeetingIssueAppService>();
            _service.UpdateNesIssueSatatus(eventParams.InstanceID.ToGuid());

        }

        /// <summary>
        /// 终止会议 会议室占用清除、终止会议通知的待办、终止会前任务的待办
        /// </summary>
        /// <param name="eventParams"></param>
        public void EndMeetingRoomUse(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLMeetingAppService>();
            _service.EndMeetingRoomUse(eventParams.InstanceID.ToGuid());
            var _userService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingUserAppService>();
            _userService.AutoInvalidMeetingUserReturnReceiptTodos(eventParams.InstanceID.ToGuid(),true);
            var _service1 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingUserBeforeTaskAppService>();
            _service1.AutoInvalidBeforeTaskTodos(eventParams.InstanceID.ToGuid());
        }

        /// <summary>
        /// 会议确认 管理员作废会议回执的待办
        /// </summary>
        public void AutoInvalidMeetingUserReturnReceiptTodos(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingUserAppService>();
            _service.AutoInvalidMeetingUserReturnReceiptTodos(eventParams.InstanceID.ToGuid());
        }

        #endregion


    }
}
