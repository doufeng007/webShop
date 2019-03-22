using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class GetXZGLMeetingRoomListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 会议室名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会议室位置
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnable { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
