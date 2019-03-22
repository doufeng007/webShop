using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class GetMeetingRoomUseInfoListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid? MeetingRoomId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

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
    public class GetMeetingRoomUseInfoByInput
    {
        #region 表字段
        public Guid MeetingRoomId { get; set; }
        public Guid BusinessId { get; set; }
        public MeetingRoomUseBusinessType BusinessType { get; set; }
        #endregion
    }
}
