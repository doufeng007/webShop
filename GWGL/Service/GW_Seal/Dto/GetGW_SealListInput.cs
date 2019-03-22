using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;

namespace GWGL
{
    public class GetGW_SealListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// Status
        /// </summary>
        public GW_SealStatusEnmu? Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        public GW_SealTypeEnmu? SealType { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
