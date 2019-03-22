using Abp.Application.Services.Dto;
using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class MeetingRoomUseInfoByEvent : EventData
    {
        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid MeetingRoomId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        public Guid BusinessId { get; set; }


        public string BusinessName { get; set; }

        public int BusinessType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        public bool IsDelete { get; set; }

    }
}
