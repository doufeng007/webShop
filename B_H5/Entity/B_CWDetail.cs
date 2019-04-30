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
    [Table("B_CWDetail")]
    public class B_CWDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// RelationUserId
        /// </summary>
        [DisplayName(@"RelationUserId")]
        public long? RelationUserId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DisplayName(@"Type")]
        public CWDetailTypeEnum Type { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [DisplayName(@"BusinessType")]
        public CWDetailBusinessTypeEnum BusinessType { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        [DisplayName(@"CategroyId")]
        public Guid CategroyId { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [DisplayName(@"Number")]
        public int Number { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        [DisplayName(@"IsDefault")]
        public bool IsDefault { get; set; }


        #endregion
    }
}