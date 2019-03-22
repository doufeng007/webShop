using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using CWGL.Enums;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [Serializable]
    [Table("CWGLPayMoney")]
    public class CWGLPayMoney : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        [MaxLength(500)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }


        /// <summary>
        /// 借款人
        /// </summary>
        [DisplayName(@"借款人")]
        [MaxLength(30)]
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [DisplayName(@"客户名称")]
        [MaxLength(30)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DisplayName(@"金额")]
        [Money]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [DisplayName(@"收款方式")]
        public MoneyMode Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [DisplayName(@"银行名称")]
        [MaxLength(100)]
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [DisplayName(@"卡号")]
        [MaxLength(64)]
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [DisplayName(@"开户行名称")]
        [MaxLength(100)]
        public string BankOpenName { get; set; }

        /// <summary>
        /// 事由摘要
        /// </summary>
        [DisplayName(@"事由摘要")]
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [DisplayName(@"电子资料")]
        public int? Nummber { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [DisplayName(@"合同编号")]
        [MaxLength(30)]
        public string ContractNum { get; set; }


        /// <summary>
        /// 银行转账流水号
        /// </summary>
        [DisplayName(@"银行转账流水号")]
        [MaxLength(30)]
        public string FlowNumber { get; set; }


        #endregion
    }
}