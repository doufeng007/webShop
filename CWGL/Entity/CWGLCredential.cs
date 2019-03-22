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
    [Table("CWGLCredential")]
    public class CWGLCredential : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 是否付款
        /// </summary>
        [DisplayName(@"是否付款")]
        public bool IsPay { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [DisplayName(@"客户名称")]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 收款事由
        /// </summary>
        [DisplayName(@"收款事由")]
        [MaxLength(200)]
        public string Cause { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [DisplayName(@"合同编号")]
        [MaxLength(30)]
        public string ContractNum { get; set; }

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
        /// 电子资料
        /// </summary>
        [DisplayName(@"电子资料")]
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        [DisplayName(@"关联编号")]
        public Guid? BusinessId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public CredentialType BusinessType { get; set; }

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
        /// 银行转账流水号
        /// </summary>
        [DisplayName(@"银行转账流水号")]
        [MaxLength(30)]
        public string FlowNumber { get; set; }

        #endregion
    }
}