using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class GetMeetingLlogisticsListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 会议类型id
        /// </summary>
        public Guid? MeetingTypeId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
