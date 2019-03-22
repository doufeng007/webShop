using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingGL
{
    [AutoMapTo(typeof(XZGLMeeting))]
    public class CreateXZGLMeetingInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(500, ErrorMessage = "名称长度必须小于500")]
        [Required(ErrorMessage = "必须填写名称")]
        public string Name { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        [Required(ErrorMessage = "必须填写会议类型")]
        public Guid MeetingTypeId { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid? RoomId { get; set; }

        /// <summary>
        /// 会议室名称
        /// </summary>
        [MaxLength(500, ErrorMessage = "会议室名称长度必须小于500")]
        public string MeetingRoomName { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 会议组织单位编号
        /// </summary>
        public long? OrgId { get; set; }

        /// <summary>
        /// 会议组织单位
        /// </summary>
        [MaxLength(500, ErrorMessage = "会议组织单位长度必须小于500")]
        public string OrganizeName { get; set; }

        /// <summary>
        /// 发起者
        /// </summary>
        [Range(0, long.MaxValue, ErrorMessage = "")]
        [Required(ErrorMessage = "必须填写发起者")]
        public long MeetingCreateUser { get; set; }

        /// <summary>
        /// 主持人编号
        /// </summary>
        [Required(ErrorMessage = "必须填写主持人")]
        public long ModeratorId { get; set; }

        /// <summary>
        /// 记录人编号
        /// </summary>
        [Required(ErrorMessage = "必须填写记录人")]
        public long RecorderId { get; set; }

        /// <summary>
        /// 参会领导
        /// </summary>
        [MaxLength(500, ErrorMessage = "参会领导长度必须小于500")]
        public string AttendingLeaders { get; set; }

        /// <summary>
        /// 会议嘉宾
        /// </summary>
        [MaxLength(500, ErrorMessage = "会议嘉宾长度必须小于500")]
        public string MeetingGuest { get; set; }

        /// <summary>
        /// 参会人员
        /// </summary>
        [MaxLength(500, ErrorMessage = "参会人员长度必须小于500")]
        [Required(ErrorMessage = "必须填写参会人员")]
        public string JoinPersonnel { get; set; }

        /// <summary>
        /// 会议主题
        /// </summary>
        [MaxLength(500, ErrorMessage = "会议主题长度必须小于500")]
        [Required(ErrorMessage = "必须填写会议主题")]
        public string MeetingTheme { get; set; }

        /// <summary>
        /// 会议议题类型
        /// </summary>
        public MeetingIsssueType MeetingIsssueType { get; set; }

        /// <summary>
        /// 会议议题
        /// </summary>
        public string MeetingIssue { get; set; }

        /// <summary>
        /// 是否需要会议资料
        /// </summary>
        public bool IsNeedFile { get; set; }

        /// <summary>
        /// 是否需要会务后勤
        /// </summary>
        public bool IsNeedLogistics { get; set; }


        public bool IsPeriod { get; set; } = false;


        public CreateMeetingPeriodRuleInput MeetingPeriodRule { get; set; } = new CreateMeetingPeriodRuleInput();



        public List<CreateXZGLMeetingIssueInput> Issues { get; set; } = new List<CreateXZGLMeetingIssueInput>();


        public List<CreateXZGLMeetingFileInput> FileList { get; set; } = new List<CreateXZGLMeetingFileInput>();

        public List<CreateXZGLMeetingLogisticsRInput> LogisticsList { get; set; } = new List<CreateXZGLMeetingLogisticsRInput>();

        #endregion
    }


    
}