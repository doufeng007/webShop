using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetCWGLPrePaymentDetailListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 预收款编号
        /// </summary>
        public Guid PrePaymentId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
