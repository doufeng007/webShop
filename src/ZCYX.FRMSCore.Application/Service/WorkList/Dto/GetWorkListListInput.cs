using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace ZCYX.FRMSCore.Application
{
    public class GetWorkListListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
