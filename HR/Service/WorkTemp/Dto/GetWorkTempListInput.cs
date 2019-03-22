using Abp.Runtime.Validation;
using HR.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetWorkTempListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public WorkTempType? Type { get; set; }
        public long? OrgId { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
