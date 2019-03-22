using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace GWGL
{
    public class GetEmployeeReceiptListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
       
        /// <summary>
        /// Status
        /// </summary>
        public int? SearchType { get; set; }
        public string DocReceiveNo { get; set; }
        public Guid? DocType { get; set; }
    }
}
