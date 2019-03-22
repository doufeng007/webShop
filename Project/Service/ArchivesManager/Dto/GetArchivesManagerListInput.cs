using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetArchivesManagerListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        public bool IncludeAudit { get; set; }


        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }

        public long? UserId { get; set; }

        public int? ArchivesManagerType { get; set; }


        public string SearchKey { get; set; }







        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
