using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace MeetingGL
{
    public class GetXZGLMeetingListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? RoomId { get; set; }
        public Guid? MeetingTypeId { get; set; }
        public DateTime? StartTime { get; set; }


        public DateTime? EndTime { get; set; }

    }
}
