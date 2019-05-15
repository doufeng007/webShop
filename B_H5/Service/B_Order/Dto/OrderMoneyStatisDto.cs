using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class OrderMoneyStatisDto
    {
        /// <summary>
        /// 总销售额
        /// </summary>
        public decimal TotalSaleAmout { get; set; }

        /// <summary>
        /// 总充值金额
        /// </summary>
        public decimal TotalPrePayAmout { get; set; }


        /// <summary>
        /// 总提现金额
        /// </summary>
        public decimal TotalWithDrawalAmout { get; set; }


        /// <summary>
        /// 总保证金金额
        /// </summary>
        public decimal TotalDeposite { get; set; }

        /// <summary>
        /// 总奖励金额
        /// </summary>
        public decimal TotalRewardAmount { get; set; }


        /// <summary>
        /// 总推荐奖金额
        /// </summary>
        public decimal TotalInviteAmout { get; set; }

        /// <summary>
        /// 总提货奖金
        /// </summary>
        public decimal TotalOrderOutBonusAmout { get; set; }


        /// <summary>
        /// 总销售返点奖金
        /// </summary>
        public decimal TotalTeamDisAmout { get; set; }

        /// <summary>
        /// 代理总余额
        /// </summary>
        public decimal TotalAgencyBlance { get; set; }



        /// <summary>
        /// 代理总货款
        /// </summary>
        public decimal TotalAgencyPayment { get; set; }

        /// <summary>
        /// 平台余额
        /// </summary>
        public decimal TotalSystemBlance { get; set; }


        /// <summary>
        /// 平台留存余额
        /// </summary>
        public decimal TotalSystemAmout { get; set; }
    }


    public class B_SyatemAmoutStatisDto
    {
        public string Date { get; set; }

        public decimal Amout { get; set; }
    }



    public class B_SyatemAmoutStatisInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }

        /// <summary>
        /// 1 是day 2是month
        /// </summary>
        public int DayOrMonth { get; set; }

        /// <summary>
        /// 空是所有代理  团队数和一级代理人数= 1
        /// </summary>
        public B_SyatemAmoutStatisType BType { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }



    public class SyatemAmoutOrderDto
    {
        public string Name { get; set; }


        public decimal Amout { get; set; }

    }
}
