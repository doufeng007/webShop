using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class GetMeetingUserListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议人员角色
        /// </summary>
        public int MeetingUserRole { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 回执状态
        /// </summary>
        public int ReturnReceiptStatus { get; set; }

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



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
