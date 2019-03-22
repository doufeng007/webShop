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
    [Table("GW_FWZHRole")]
    public class GW_FWZHRole : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Name
        /// </summary>
        [DisplayName(@"Name")]
        [MaxLength(1000)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [DisplayName(@"Code")]
        [MaxLength(1000)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// StartIndex
        /// </summary>
        [DisplayName(@"StartIndex")]
        public int StartIndex { get; set; }

        /// <summary>
        /// AutoCoding
        /// </summary>
        [DisplayName(@"AutoCoding")]
        public bool AutoCoding { get; set; }


        #endregion
    }
}