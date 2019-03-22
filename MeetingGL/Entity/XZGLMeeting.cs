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

namespace MeetingGL
{
    [Serializable]
    [Table("XZGLMeeting")]
    public class XZGLMeeting : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName(@"名称")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }


        public bool IsPeriod { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        [DisplayName(@"会议类型")]
        public Guid MeetingTypeId { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        [DisplayName(@"会议室编号")]
        public Guid? RoomId { get; set; }

        /// <summary>
        /// 会议室名称
        /// </summary>
        [DisplayName(@"会议室名称")]
        [MaxLength(500)]
        public string MeetingRoomName { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        [DisplayName(@"会议开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        [DisplayName(@"会议结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 会议组织单位编号
        /// </summary>
        [DisplayName(@"会议组织单位编号")]
        public long? OrgId { get; set; }

        /// <summary>
        /// 会议组织单位
        /// </summary>
        [DisplayName(@"会议组织单位")]
        [MaxLength(500)]
        public string OrganizeName { get; set; }

        /// <summary>
        /// 发起者
        /// </summary>
        [DisplayName(@"发起者")]
        public long MeetingCreateUser { get; set; }

        /// <summary>
        /// 主持人编号
        /// </summary>
        [DisplayName(@"主持人编号")]
        public long ModeratorId { get; set; }

        /// <summary>
        /// 记录人编号
        /// </summary>
        [DisplayName(@"记录人编号")]
        public long RecorderId { get; set; }

        /// <summary>
        /// 参会领导
        /// </summary>
        [DisplayName(@"参会领导")]
        [MaxLength(500)]
        public string AttendingLeaders { get; set; }

        /// <summary>
        /// 会议嘉宾
        /// </summary>
        [DisplayName(@"会议嘉宾")]
        [MaxLength(500)]
        public string MeetingGuest { get; set; }

        /// <summary>
        /// 参会人员
        /// </summary>
        [DisplayName(@"参会人员")]
        [MaxLength(500)]
        public string JoinPersonnel { get; set; }

        /// <summary>
        /// 会议主题
        /// </summary>
        [DisplayName(@"会议主题")]
        [MaxLength(500)]
        public string MeetingTheme { get; set; }

        /// <summary>
        /// 其他人员
        /// </summary>
        [DisplayName(@"其他人员")]
        [MaxLength(500)]
        public string OtherPersonnel { get; set; }

        /// <summary>
        /// 会议记录
        /// </summary>
        [DisplayName(@"会议记录")]
        public string Record { get; set; }

        /// <summary>
        /// 会议议题类型
        /// </summary>
        [DisplayName(@"会议议题类型")]
        public MeetingIsssueType MeetingIsssueType { get; set; }

        /// <summary>
        /// 会议议题
        /// </summary>
        [DisplayName(@"会议议题")]
        public string MeetingIssue { get; set; }

        /// <summary>
        /// 是否需要会议资料
        /// </summary>
        [DisplayName(@"是否需要会议资料")]
        public bool IsNeedFile { get; set; }

        /// <summary>
        /// 是否需要会务后勤
        /// </summary>
        [DisplayName(@"是否需要会务后勤")]
        public bool IsNeedLogistics { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        [MaxLength(500)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// CopyForUsers
        /// </summary>
        [DisplayName(@"CopyForUsers")]
        [MaxLength(500)]
        public string CopyForUsers { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName(@"实际与会人员")]
        public string RealAttendeeUsers { get; set; }

        /// <summary>
        /// 实际参会嘉宾
        /// </summary>
        [DisplayName(@"实际参会嘉宾")]
        public string RealMeetingGuest { get; set; }

        /// <summary>
        /// 缺席人
        /// </summary>
        [DisplayName(@"缺席人")]
        public string AbsentUser { get; set; }


        /// <summary>
        /// 是否提交会议记录
        /// </summary>
        public bool? HasSubmitRecord { get; set; } = false;


        /// <summary>
        /// 提醒编号
        /// </summary>
        [DisplayName(@"提醒编号")]
        public string HangfireJobId { get; set; }
        #endregion
    }
}