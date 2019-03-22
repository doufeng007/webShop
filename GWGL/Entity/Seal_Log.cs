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
    [Table("Seal_Log")]
    public class Seal_Log : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Seal_Id
        /// </summary>
        [DisplayName(@"Seal_Id")]
        public Guid Seal_Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DisplayName(@"Title")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Copies
        /// </summary>
        [DisplayName(@"Copies")]
        public int Copies { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        public string Remark { get; set; }


        #endregion
    }
}