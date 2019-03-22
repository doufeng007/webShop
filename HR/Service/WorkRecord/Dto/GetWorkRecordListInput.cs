using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetWorkRecordListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public string Status { get; set; }

        public Guid? BusinessId { get; set; }


        public CollaborativePersonnel BusinessType { get; set; }

    }
}
