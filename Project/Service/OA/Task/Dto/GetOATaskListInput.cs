using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetOATaskListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        public string Status { get; set; }

        public bool ValByCurrentUser { get; set; } = true;


        public bool CreateByCurrentUser { get; set; } = true;

        public bool HasParticipate { get; set; } = true;

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime desc";
            }
        }
    }
}
