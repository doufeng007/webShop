using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{
    public class GetUserSupplyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long MenuId { get; set; }

        public bool IncludeDetele { get; set; }

        public string Status { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime desc";
            }
        }
    }
}
