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
    /// <summary>
    /// 实体店报名
    /// </summary>
    [Serializable]
    [Table("B_StoreSignUp")]
    public class B_StoreSignUp : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long? UserId { get; set; }

        /// <summary>
        /// Provinces
        /// </summary>
        [DisplayName(@"Provinces")]
        [MaxLength(100)]
        public string Provinces { get; set; }

        /// <summary>
        /// County
        /// </summary>
        [DisplayName(@"County")]
        [MaxLength(100)]
        public string County { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [DisplayName(@"City")]
        [MaxLength(100)]
        public string City { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DisplayName(@"Address")]
        [MaxLength(500)]
        public string Address { get; set; }

        /// <summary>
        /// BankNumber
        /// </summary>
        [DisplayName(@"BankNumber")]
        [MaxLength(50)]
        public string BankNumber { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        [DisplayName(@"BankUserName")]
        [MaxLength(50)]
        public string BankUserName { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [DisplayName(@"BankName")]
        [MaxLength(100)]
        public string BankName { get; set; }

        /// <summary>
        /// OpenDate
        /// </summary>
        [DisplayName(@"OpenDate")]
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// StorArea
        /// </summary>
        [DisplayName(@"StorArea")]
        [MaxLength(50)]
        public string StorArea { get; set; }

        /// <summary>
        /// Goods
        /// </summary>
        [DisplayName(@"Goods")]
        public string Goods { get; set; }


        #endregion
    }
}