using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetOAContractCollectionFeeListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }

        public DateTime? StarTime { get; set; }


        public DateTime? EndTime { get; set; }


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
