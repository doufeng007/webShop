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
    /// 我的地址
    /// </summary>
    [Serializable]
    [Table("B_MyAddress")]
    public class B_MyAddress : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

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
        /// Addres
        /// </summary>
        [DisplayName(@"Addres")]
        [MaxLength(500)]
        [Required]
        public string Addres { get; set; }

        /// <summary>
        /// Consignee
        /// </summary>
        [DisplayName(@"Consignee")]
        [MaxLength(100)]
        [Required]
        public string Consignee { get; set; }

        /// <summary>
        /// Tel
        /// </summary>
        [DisplayName(@"Tel")]
        [MaxLength(50)]
        [Required]
        public string Tel { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        [DisplayName(@"IsDefault")]
        public bool IsDefault { get; set; }


        #endregion
    }
}