using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_AgencyUpgrade))]
    public class B_AgencyUpgradeOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// AgencyId
        /// </summary>
        public Guid AgencyId { get; set; }

        /// <summary>
        /// ToAgencyLevelId
        /// </summary>
        public Guid ToAgencyLevelId { get; set; }

        public string ToAgencyLevelName { get; set; }

        /// <summary>
        /// NeedPrePayAmout
        /// </summary>
        public decimal NeedPrePayAmout { get; set; }

        /// <summary>
        /// NeedDeposit
        /// </summary>
        public decimal NeedDeposit { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 打款方式  1支付宝 2银行转账
        /// </summary>
        public PayAccountType PayType { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        public decimal PayAmout { get; set; }

        /// <summary>
        /// 打款账户
        /// </summary>
        public string PayAcount { get; set; }

        /// <summary>
        /// 银行户名
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string BankName { get; set; }


        /// <summary>
        /// 打款日期
        /// </summary>
        public DateTime PayDate { get; set; }


        /// <summary>
        /// 打款凭证
        /// </summary>
        public List<GetAbpFilesOutput> CredentFiles { get; set; } = new List<GetAbpFilesOutput>();


        /// <summary>
        /// 手持凭证
        /// </summary>
        public List<GetAbpFilesOutput> HandleCredentFiles { get; set; } = new List<GetAbpFilesOutput>();



    }
}
