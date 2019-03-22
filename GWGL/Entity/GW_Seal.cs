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
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    [Serializable]
    [Table("GW_Seal")]
    public class GW_Seal : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Name
        /// </summary>
        [DisplayName(@"Name")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// KeepUser
        /// </summary>
        [DisplayName(@"KeepUser")]
        public long KeepUser { get; set; }

        /// <summary>
        /// AuditUser
        /// </summary>
        [DisplayName(@"AuditUser")]
        [MaxLength(100)]
        [Required]
        public string AuditUser { get; set; }

        /// <summary>
        /// SealType
        /// </summary>
        [DisplayName(@"SealType")]
        public GW_SealTypeEnmu SealType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public GW_SealStatusEnmu Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        public string Remark { get; set; }


        #endregion
    }
}