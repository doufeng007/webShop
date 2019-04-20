using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_WithdrawalListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 1 支付宝 2银行转账
        /// </summary>
        public int? PayType { get; set; }

        /// <summary>
        /// 代理等级
        /// </summary>
        public Guid? AgencyLevelId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public B_WithdrawalStatusEnum? Status { get; set; }

        /// <summary>
        /// 打款日期-开始日期
        /// </summary>
        public DateTime? PayDateStart { get; set; }

        /// <summary>
        /// 打款日期-结束日期
        /// </summary>
        public DateTime? PayDateEnd { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
