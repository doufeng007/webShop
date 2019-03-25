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
    /// 试用装
    /// </summary>
    [Serializable]
    [Table("B_TrialProduct")]
    public class B_TrialProduct : FullAuditedEntity<Guid>, IMayHaveTenant
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
        /// IsActive
        /// </summary>
        [DisplayName(@"IsActive")]
        public bool IsActive { get; set; }


        #endregion
    }
}