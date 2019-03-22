using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingUser))]
    public class CreateMeetingUserInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议人员角色
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public MeetingUserRole MeetingUserRole { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }
        
        #endregion
    }
}