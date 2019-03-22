using Abp.Application.Services.Dto;
using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore
{
    public class TaskManagementData : EventData
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        ///任务状态
        /// </summary>
        public TaskManagementStateEnum TaskStatus { get; set; }
    }

}
