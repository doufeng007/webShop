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
    [Table("B_ManagerPayAccount")]
    public class B_ManagerPayAccount : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// Type
        /// </summary>
        [DisplayName(@"Type")]
        public PayAccountType Type { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        [DisplayName(@"Account")]
        [MaxLength(100)]
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [DisplayName(@"BankName")]
        [MaxLength(50)]
        [Required]
        public string BankName { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        [DisplayName(@"BankUserName")]
        [MaxLength(50)]
        [Required]
        public string BankUserName { get; set; }

        /// <summary>
        /// WxName
        /// </summary>
        [DisplayName(@"WxName")]
        [MaxLength(50)]
        [Required]
        public string WxName { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(200)]
        [Required]
        public string Remark { get; set; }

        public PayAccountStatus Status { get; set; }


        #endregion
    }
}