using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class XZGLMeetingRoomTimeListOutput : PagedAndSortedInputDto, IShouldNormalize
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 会议室名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public List<XZGLMeetingListTimeOutput> Times { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class XZGLMeetingListTimeOutput
    {

        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public MeetingRoomUseBusinessType Type { get; set; }
        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
