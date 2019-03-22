using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetFinancialAccountingCertificateListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }


    public class GetByBusinessIdInput
    {
        public FACertificateBusinessType BusinessType { get; set; }

        public string BusinessId { get; set; }


        public string Content { get; set; }
    }


    public class CLSystemReturnInfo
    {
        public bool result { get; set; }
    }

    public class CLResultInfo : CLSystemReturnInfo
    {
        public string Id { get; set; }




        public List<CLResultDetailInfo> Content { get; set; } = new List<CLResultDetailInfo>();
    }

    public class CLResultDetailInfo
    {
        public Guid ASid { get; set; }

        public int FACType { get; set; }


        public decimal Money { get; set; }
    }
}
