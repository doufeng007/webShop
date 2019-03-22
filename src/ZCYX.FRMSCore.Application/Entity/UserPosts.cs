using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [Table("UserPosts")]
    public class UserPosts : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long UserId { get; set; }

        public Guid PostId { get; set; }

        public Guid OrgPostId { get; set; }

        public long OrgId { get; set; }
        /// <summary>
        /// 是否主岗位
        /// </summary>
        public bool IsMain { get; set; }
    }
}
