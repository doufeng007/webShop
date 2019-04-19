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

namespace B_H5
{
    [Serializable]
    [Table("B_PaymentPrepay")]
    public class B_PaymentPrepay : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [DisplayName(@"Code")]
        [MaxLength(100)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        [DisplayName(@"PayType")]
        public int PayType { get; set; }

        /// <summary>
        /// PayAmout
        /// </summary>
        [DisplayName(@"PayAmout")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [DisplayName(@"BankName")]
        [MaxLength(50)]
        public string BankName { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        [DisplayName(@"BankUserName")]
        [MaxLength(100)]
        public string BankUserName { get; set; }

        /// <summary>
        /// PayAcount
        /// </summary>
        [DisplayName(@"PayAcount")]
        [MaxLength(50)]
        [Required]
        public string PayAcount { get; set; }

        /// <summary>
        /// PayDate
        /// </summary>
        [DisplayName(@"PayDate")]
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public B_PrePayStatusEnum Status { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [DisplayName(@"Reason")]
        [MaxLength(500)]
        public string Reason { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// AuditRemark
        /// </summary>
        [DisplayName(@"AuditRemark")]
        [MaxLength(1000)]
        public string AuditRemark { get; set; }


        #endregion
    }
}