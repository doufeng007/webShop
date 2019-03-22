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

namespace Project
{
    [Serializable]
    [Table("QuickLinkUser")]
    public class QuickLinkUser : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 快捷入口编号
        /// </summary>
        [DisplayName(@"快捷入口编号")]
        public Guid QuickLinkId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [DisplayName(@"用户")]
        public long UserId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DisplayName(@"序号")]
        public int Sort { get; set; }


        #endregion
    }
}