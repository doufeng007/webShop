using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingRoomUseInfo))]
    public class CreateMeetingRoomUseInfoInput 
    {
        #region 表字段
        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid MeetingRoomId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        public Guid BusinessId { get; set; }


        public string BusinessName { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public MeetingRoomUseBusinessType BusinessType { get; set; }

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
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Status { get; set; }


		
        #endregion
    }
}