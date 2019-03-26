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
    /// 代理
    /// </summary>
    [Serializable]
    [Table("B_Agency")]
    public class B_Agency : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// AgencyLevel
        /// </summary>
        [DisplayName(@"AgencyLevel")]
        public Guid AgencyLevel { get; set; }

        /// <summary>
        /// AgenCyCode
        /// </summary>
        [DisplayName(@"AgenCyCode")]
        [MaxLength(100)]
        [Required]
        public string AgenCyCode { get; set; }

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
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DisplayName(@"Type")]
        public B_AgencyTypeEnum Type { get; set; }

        /// <summary>
        /// SignData
        /// </summary>
        [DisplayName(@"SignData")]
        public DateTime SignData { get; set; }

        /// <summary>
        /// Agreement
        /// </summary>
        [DisplayName(@"Agreement")]
        public string Agreement { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [DisplayName(@"OpenId")]
        [MaxLength(200)]
        public string OpenId { get; set; }

        /// <summary>
        /// UnitId
        /// </summary>
        [DisplayName(@"UnitId")]
        [MaxLength(200)]
        public string UnitId { get; set; }


        #endregion
    }
}