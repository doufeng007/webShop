using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetFACertificateDetailListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 记账凭证主表id
        /// </summary>
        public Guid MainId { get; set; }

        /// <summary>
        /// 会计科目id
        /// </summary>
        public Guid AccountingCourseId { get; set; }

        /// <summary>
        /// 借贷类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
