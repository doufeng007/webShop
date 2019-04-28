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
    [Table("B_AgencyDisableRecord")]
    public class B_AgencyDisableRecord : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// AgencyId
        /// </summary>
        [DisplayName(@"AgencyId")]
        public Guid AgencyId { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [DisplayName(@"Reason")]
        [MaxLength(500)]
        [Required]
        public string Reason { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(500)]
        [Required]
        public string Remark { get; set; }


        #endregion
    }
}