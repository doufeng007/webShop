using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace MeetingGL
{
	[AutoMapFrom(typeof(XZGLMeeting))]
    public class XZGLMeetingChangeDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议名称
        /// </summary>
        [LogColumn(@"会议名称", IsLog = true)]
        public string Name { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        [LogColumn(@"会议室", IsLog = true)]
        public string MeetingRoomName { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        [LogColumn(@"会议类型", IsLog = true)]
        public string MeetingTypeName { get; set; }


        /// <summary>
        /// 会议开始时间
        /// </summary>
        [LogColumn(@"会议开始时间", IsLog = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        [LogColumn(@"会议结束时间", IsLog = true)]
        public DateTime EndTime { get; set; }



        /// <summary>
        /// 会议组织单位
        /// </summary>
        [LogColumn(@"会议组织单位", IsLog = true)]
        public string OrganizeName { get; set; }

        /// <summary>
        /// 主持人姓名
        /// </summary>
        [LogColumn(@"主持人姓名", IsLog = true)]
        public string ModeratorName { get; set; }

        /// <summary>
        /// 记录人姓名
        /// </summary>
        [LogColumn(@"记录人姓名", IsLog = true)]
        public string RecorderName { get; set; }


        [LogColumn(@"参会领导", IsLog = true)]
        public string AttendingLeadersName { get; set; }

        /// <summary>
        /// 会议嘉宾
        /// </summary>
        [LogColumn(@"参会嘉宾", IsLog = true)]
        public string MeetingGuest { get; set; }

        /// <summary>
        /// 会议主题
        /// </summary>
        [LogColumn(@"会议主题", IsLog = true)]
        public string MeetingTheme { get; set; }



        #endregion
    }
}