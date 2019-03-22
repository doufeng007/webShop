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

namespace ZCYX.FRMSCore.Application
{
    [Serializable]
    [Table("WorkList")]
    public class WorkList : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 返回编号
        /// </summary>
        [DisplayName(@"返回编号")]
        [MaxLength(32)]
        public string BackId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName(@"内容")]
        public string Content { get; set; }


        #endregion
    }
}