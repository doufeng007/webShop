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
    /// 推广链接
    /// </summary>
    [Serializable]
    [Table("B_InviteUrl")]
    public class B_InviteUrl : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// AgencyLevel
        /// </summary>
        [DisplayName(@"AgencyLevel")]
        public Guid AgencyLevel { get; set; }

        /// <summary>
        /// ValidityDataType
        /// </summary>
        [DisplayName(@"ValidityDataType")]
        public int ValidityDataType { get; set; }

        /// <summary>
        /// AvailableCount
        /// </summary>
        [DisplayName(@"AvailableCount")]
        public int AvailableCount { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DisplayName(@"Url")]
        [MaxLength(200)]
        [Required]
        public string Url { get; set; }


        #endregion
    }
}