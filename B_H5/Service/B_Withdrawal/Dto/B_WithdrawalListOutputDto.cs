using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_Withdrawal))]
    public class B_WithdrawalListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string UserName { get; set; }

        public string Tel { get; set; }

        public Guid AgencyLevelId { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 支行名称
        /// </summary>
        public string BankBranchName { get; set; }

        /// <summary>
        /// 开号姓名
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string BankNumber { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amout { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 打款日期
        /// </summary>
        public DateTime? PayTime { get; set; }

        public B_WithdrawalStatusEnum Status { get; set; }


    }


    public class B_WithdrawalCount
    {
        /// <summary>
        /// 待打款人数
        /// </summary>
        public int WaitAuditCount { get; set; }

        /// <summary>
        /// 已打款人数
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// 异常人数
        /// </summary>
        public int NoPassCount { get; set; }

        /// <summary>
        /// 已打款金额
        /// </summary>
        public decimal PassAmout { get; set; }

        /// <summary>
        /// 待打款金额
        /// </summary>
        public decimal WaitAmout { get; set; }
    }
}
