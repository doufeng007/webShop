using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class GetMeetingUserBeforeTaskListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

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
