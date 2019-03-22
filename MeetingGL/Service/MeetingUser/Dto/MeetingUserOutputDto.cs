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
    [AutoMapFrom(typeof(MeetingUser))]
    public class MeetingUserOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议人员角色
        /// </summary>
        public MeetingUserRole MeetingUserRole { get; set; }


        public string MeetingUserRoleName { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }


        public string UserName { get; set; }


        public string WorkNumber { get; set; }

        /// <summary>
        /// 回执状态
        /// </summary>
        public ReturnReceiptStatus ReturnReceiptStatus { get; set; }

        public string ReturnReceiptStatusName { get; set; }

        /// <summary>
        /// 回执时间
        /// </summary>
        public DateTime? ConfirmData { get; set; }

        /// <summary>
        /// 请假备注
        /// </summary>
        public string AskForLeaveRemark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }


        public string StatusRemark { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public XZGLMeetingOutputDto MeetingInfo = new XZGLMeetingOutputDto();


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }


    [AutoMapFrom(typeof(MeetingUser))]
    public class MeetingUserOutputDtoForView : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议人员角色
        /// </summary>
        public MeetingUserRole MeetingUserRole { get; set; }


        public string MeetingUserRoleName { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }


        public string UserName { get; set; }


        public string WorkNumber { get; set; }

        /// <summary>
        /// 回执状态
        /// </summary>
        public ReturnReceiptStatus ReturnReceiptStatus { get; set; }

        public string ReturnReceiptStatusName { get; set; }

        /// <summary>
        /// 回执时间
        /// </summary>
        public DateTime? ConfirmData { get; set; }

        /// <summary>
        /// 请假备注
        /// </summary>
        public string AskForLeaveRemark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }


        public string StatusRemark { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
