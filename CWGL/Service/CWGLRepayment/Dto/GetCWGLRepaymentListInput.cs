using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetCWGLRepaymentListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 借款编号
        /// </summary>
        public Guid BorrowMoneyId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
    public class GetRepaymentListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public bool GetMy { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
