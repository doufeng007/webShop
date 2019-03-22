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

namespace CWGL
{
    [Serializable]
    [Table("FinancialAccountingCertificate")]
    public class FinancialAccountingCertificate : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编码
        /// </summary>
        [DisplayName(@"编码")]
        [MaxLength(200)]
        public string Code { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [DisplayName(@"业务类型")]
        public int BusinessType { get; set; }

        /// <summary>
        /// 业务编码
        /// </summary>
        [DisplayName(@"业务编码")]
        [MaxLength(100)]
        public string BusinessId { get; set; }

        /// <summary>
        /// 业务关联用户
        /// </summary>
        [DisplayName(@"业务关联用户")]
        public long? UserId { get; set; }

        /// <summary>
        /// 用户所属部门
        /// </summary>
        [DisplayName(@"用户所属部门")]
        public long? OrgId { get; set; }

        /// <summary>
        /// 记账人员
        /// </summary>
        [DisplayName(@"记账人员")]
        public long? KeepUserId { get; set; }

        /// <summary>
        /// 审核人员
        /// </summary>
        [DisplayName(@"审核人员")]
        public long? AuditUserId { get; set; }

        /// <summary>
        /// 出纳人员
        /// </summary>
        [DisplayName(@"出纳人员")]
        public long? CashierUserId { get; set; }

        /// <summary>
        /// 制单人员
        /// </summary>
        [DisplayName(@"制单人员")]
        public long? MakeUserId { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DisplayName(@"摘要")]
        [MaxLength(1000)]
        public string Summary { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName(@"项目名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [DisplayName(@"地区")]
        [MaxLength(50)]
        public string Region { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DisplayName(@"类别")]
        public Guid? Type { get; set; }

        [DisplayName(@"识别结果id")]
        public string ResultId { get; set; }

        /// <summary>
        /// 借方金额合计
        /// </summary>
        [DisplayName(@"借方金额合计")]
        public decimal TotalDebitAmount { get; set; }

        /// <summary>
        /// 贷方金额合计
        /// </summary>
        [DisplayName(@"贷方金额合计")]
        public decimal TotalCreditAmount { get; set; }

        /// <summary>
        /// 是否会计确认
        /// </summary>
        [DisplayName(@"是否会计确认")]
        public bool IsAccounting { get; set; }


        #endregion
    }
}