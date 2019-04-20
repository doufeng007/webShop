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
    [Table("B_Withdrawal")]
    public class B_Withdrawal : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段


        /// <summary>
        /// Code
        /// </summary>
        [DisplayName(@"Code")]
        [MaxLength(100)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [DisplayName(@"BankName")]
        [MaxLength(100)]
        [Required]
        public string BankName { get; set; }

        /// <summary>
        /// BankBranchName
        /// </summary>
        [DisplayName(@"BankBranchName")]
        [MaxLength(200)]
        [Required]
        public string BankBranchName { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        [DisplayName(@"BankUserName")]
        [MaxLength(50)]
        [Required]
        public string BankUserName { get; set; }

        /// <summary>
        /// BankNumber
        /// </summary>
        [DisplayName(@"BankNumber")]
        [MaxLength(50)]
        [Required]
        public string BankNumber { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        [DisplayName(@"Amout")]
        public decimal Amout { get; set; }

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


        public B_WithdrawalStatusEnum Status { get; set; }


        public string AuditRemark { get; set; }

        public DateTime? PayTime { get; set; }


        #endregion
    }
}