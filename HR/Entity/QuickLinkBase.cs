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

namespace HR
{
    [Serializable]
    [Table("QuickLinkBase")]
    public class QuickLinkBase : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName(@"名称")]
        [MaxLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [DisplayName(@"链接")]
        [MaxLength(500)]
        public string Link { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DisplayName(@"序号")]
        public int Sort { get; set; }


        #endregion
    }
}