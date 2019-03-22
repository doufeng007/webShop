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
    [Table("FACertificateDetail")]
    public class FACertificateDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 记账凭证主表id
        /// </summary>
        [DisplayName(@"记账凭证主表id")]
        public Guid MainId { get; set; }

        /// <summary>
        /// 会计科目id
        /// </summary>
        [DisplayName(@"会计科目id")]
        public Guid AccountingCourseId { get; set; }

        /// <summary>
        /// 借贷类型
        /// </summary>
        [DisplayName(@"借贷类型")]
        public int BusinessType { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DisplayName(@"摘要")]
        public string Summary { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DisplayName(@"金额")]
        public decimal Amount { get; set; }


        #endregion
    }
}