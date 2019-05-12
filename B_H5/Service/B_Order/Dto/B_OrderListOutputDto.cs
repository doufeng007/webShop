using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application.Dto;
using Abp.Runtime.Validation;

namespace B_H5
{
    [AutoMapFrom(typeof(B_Order))]
    public class B_OrderListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal Amout { get; set; }

        /// <summary>
        /// Stauts
        /// </summary>
        public int Stauts { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }



    public class GetAgencyMoneyStaticInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public Guid? LeavelId { get; set; }


        


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class AgencyMoneyStaticDto
    {
        public string AgencyCode { get; set; }


        public string AgencyName { get; set; }



        /// <summary>
        /// 代理级别
        /// </summary>
        public string AgencyLevelName { get; set; }


        public Guid AgencyLevelId { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal Deposite { get; set; }

        /// <summary>
        /// 进货金额
        /// </summary>
        public decimal OrderInAmout { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal WithdrawalAmout { get; set; }


        /// <summary>
        /// 下级提货奖
        /// </summary>
        public decimal ChildOrderinOutAmout { get; set; }

        /// <summary>
        /// 推荐奖
        /// </summary>
        public decimal InviteAmout { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Blance { get; set; }


        /// <summary>
        ///货款
        /// </summary>
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 团队奖金
        /// </summary>
        public decimal TeamBonus { get; set; }


        public DateTime AgencyCreateTime { get; set; }

    }



    public class AgencyMoneyDetailListDto
    {
        public OrderAmoutBusinessTypeEnum BusinessType { get; set; }


        public string BusinessTitle { get; set; }
    }
}
