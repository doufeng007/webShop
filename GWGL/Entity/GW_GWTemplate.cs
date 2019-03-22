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
    [Table("GW_GWTemplate")]
    public class GW_GWTemplate : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Title
        /// </summary>
        [DisplayName(@"Title")]
        [MaxLength(500)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DisplayName(@"Content")]
        [Required]
        public string Content { get; set; }


        #endregion
    }
}