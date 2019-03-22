using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace MeetingGL
{
    [AutoMapFrom(typeof(XZGLMeeting))]
    public class XZGLMeetingOutputDto : WorkFlowTaskCommentResult
    {
        #region 会议信息
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        public Guid MeetingTypeId { get; set; }


        public string MeetingTypeName { get; set; }


        public bool ReturnReceiptEnable { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid? RoomId { get; set; }

        /// <summary>
        /// 会议室名称
        /// </summary>
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
        public string OrganizeName { get; set; }

        /// <summary>
        /// 发起者
        /// </summary>
        public long MeetingCreateUser { get; set; }


        public string MeetingCreateUser_Name { get; set; }

        /// <summary>
        /// 主持人编号
        /// </summary>
        public long ModeratorId { get; set; }


        public string ModeratorId_Name { get; set; }

        /// <summary>
        /// 记录人编号
        /// </summary>
        public long RecorderId { get; set; }


        public string RecorderName { get; set; }

        /// <summary>
        /// 参会领导
        /// </summary>
        public string AttendingLeaders { get; set; }


        public string AttendingLeadersName { get; set; }

        /// <summary>
        /// 会议嘉宾
        /// </summary>
        public string MeetingGuest { get; set; }

        /// <summary>
        /// 参会人员
        /// </summary>
        public string JoinPersonnel { get; set; }

        public string JoinPersonnelName { get; set; }

        /// <summary>
        /// 会议主题
        /// </summary>
        public string MeetingTheme { get; set; }

        /// <summary>
        /// 其他人员
        /// </summary>
        public string OtherPersonnel { get; set; }

        /// <summary>
        /// 会议记录
        /// </summary>
        public string Record { get; set; }

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

        /// <summary>
        /// DealWithUsers
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// CopyForUsers
        /// </summary>
        public string CopyForUsers { get; set; }

        public string CopyForUsersName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public bool IsPeriod { get; set; } = false;


        public MeetingPeriodRuleOutputDto MeetingPeriodRule { get; set; } = new MeetingPeriodRuleOutputDto();


        public List<MeetingIssueOutputDto> IssueList { get; set; } = new List<MeetingIssueOutputDto>();


        public List<XZGLMeetingFileOutput> FileList { get; set; } = new List<XZGLMeetingFileOutput>();


        public List<XZGLMeetingLogisticsROutput> LogisticsList { get; set; } = new List<XZGLMeetingLogisticsROutput>();


        public List<MeetingUserOutputDtoForView> MeetingUserReturnReceipt { get; set; } = new List<MeetingUserOutputDtoForView>();


        public List<MeetingUserOutputDtoForView> MeetingUsers { get; set; } = new List<MeetingUserOutputDtoForView>();


        public ReturnReceiptStatus CurrntUserReturnReceiptStatus { get; set; }

        public string CurrntUserReturnReceiptStatusName { get; set; }

        /// <summary>
        /// 应到人数
        /// </summary>
        public int MeetingUserShouldCount { get; set; }

        /// <summary>
        /// 请假人数
        /// </summary>
        public int MeetingUserAbsentCount { get; set; }


        /// <summary>
        ///实际与会人员
        /// </summary>
        public string RealAttendeeUsers { get; set; }

        /// <summary>
        ///实际与会人员
        /// </summary>
        public string RealAttendeeUsers_Name { get; set; }

        /// <summary>
        /// 实际参会嘉宾
        /// </summary>
        public string RealMeetingGuest { get; set; }



        /// <summary>
        /// 缺席人
        /// </summary>
        public string AbsentUser { get; set; }

        /// <summary>
        /// 缺席人
        /// </summary>
        public string AbsentUserName { get; set; }

        /// <summary>
        /// 是否提交会议记录
        /// </summary>
        public bool? HasSubmitRecord { get; set; } = false;



        #endregion
    }



}
