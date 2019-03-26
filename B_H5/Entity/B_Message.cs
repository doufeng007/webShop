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
    /// 代理消息
    /// </summary>
    [Serializable]
    [Table("B_Message")]
    public class B_Message : FullAuditedEntity<Guid>, IMayHaveTenant
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
        /// Code
        /// </summary>
        [DisplayName(@"Code")]
        [MaxLength(100)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [DisplayName(@"BusinessType")]
        public B_H5MesagessType BusinessType { get; set; }

        /// <summary>
        /// BusinessId
        /// </summary>
        [DisplayName(@"BusinessId")]
        public Guid BusinessId { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DisplayName(@"Content")]
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        #endregion
    }
}