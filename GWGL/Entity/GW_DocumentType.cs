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

namespace GWGL
{
    [Serializable]
    [Table("GW_DocumentType")]
    public class GW_DocumentType : FullAuditedEntity<Guid>, IMayHaveTenant
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
        /// Type
        /// </summary>
        [DisplayName(@"Type")]
        public int Type { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        #endregion
    }
}