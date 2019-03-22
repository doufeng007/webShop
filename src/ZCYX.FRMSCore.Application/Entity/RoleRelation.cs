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
    [Table("RoleRelation")]
    public class RoleRelation : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 关联id
        /// </summary>
        [DisplayName(@"关联id")]
        public Guid RelationId { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        [DisplayName(@"关联类型")]
        public RelationType Type { get; set; }

        /// <summary>
        /// 关联用户
        /// </summary>
        [DisplayName(@"关联用户")]
        public long RelationUserId { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        [DisplayName(@"当前用户")]
        public long UserId { get; set; }

        /// <summary>
        /// 转让角色
        /// </summary>
        [DisplayName(@"转让角色")]
        public string Roles { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName(@"开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName(@"结束时间")]
        public DateTime EndTime { get; set; }


        #endregion
    }
}