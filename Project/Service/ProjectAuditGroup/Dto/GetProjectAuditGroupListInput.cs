using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetProjectAuditGroupListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }
    }


    public class CopyForProjectAuditGroupInput
    {
        public string InstanceID { get; set; }

        public Guid TaskID { get; set; }


        public Guid FlowID { get; set; }

        public Guid GroupID { get; set; }

        public bool HasFinancialReviewMember { get; set; }
    }
}
