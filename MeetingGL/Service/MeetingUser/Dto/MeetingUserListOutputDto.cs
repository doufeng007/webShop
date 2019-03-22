using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace MeetingGL
{
    [AutoMapFrom(typeof(MeetingUser))]
    public class MeetingUserListOutputDto : BusinessWorkFlowListOutput
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

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 回执状态
        /// </summary>
        public ReturnReceiptStatus ReturnReceiptStatus { get; set; }

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

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
