using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace IMLib
{
    public class GetIM_InquiryListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 待办Id
        /// </summary>
        public Guid TaskId { get; set; }

        public Guid FlowId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
