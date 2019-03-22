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
    [Table("CWGLBorrowMoney")]
    public class CWGLBorrowMoney : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 借款人
        /// </summary>
        [DisplayName(@"借款人")]
        public long UserId { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [DisplayName(@"部门编号")]
        public long OrgId { get; set; }

        /// <summary>
        /// 借款类型
        /// </summary>
        [DisplayName(@"借款类型")]
        public BorrowMoney TypeId { get; set; }

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
        /// 还款日期
        /// </summary>
        [DisplayName(@"还款日期")]
        public DateTime? RepayTime { get; set; }

        /// <summary>
        /// 还款方式
        /// </summary>
        [DisplayName(@"还款方式")]
        public MoneyMode? RepayMode { get; set; }

        /// <summary>
        /// 还款银行名称
        /// </summary>
        [DisplayName(@"还款银行名称")]
        [MaxLength(100)]
        public string RepayBankName { get; set; }

        /// <summary>
        /// 还款卡号
        /// </summary>
        [DisplayName(@"还款卡号")]
        [MaxLength(64)]
        public string RepayCardNumber { get; set; }

        /// <summary>
        /// 还款开户行名称
        /// </summary>
        [DisplayName(@"还款开户行名称")]
        [MaxLength(100)]
        public string RepayBankOpenName { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [DisplayName(@"电子资料")]
        public int? Nummber { get; set; }

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
        public int Status { get; set; }

        /// <summary>
        /// 还款时间
        /// </summary>
        [DisplayName(@"还款时间")]
        public DateTime? RepaymentTime { get; set; }
        
        /// <summary>
        /// IsPayBack
        /// </summary>
        [DisplayName(@"是否归还")]
        public bool IsPayBack { get; set; }


        #endregion
    }
}