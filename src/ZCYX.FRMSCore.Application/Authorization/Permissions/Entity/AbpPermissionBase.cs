using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZCYX.FRMSCore
{
    [Table("AbpPermissionBase")]
    public class AbpPermissionBase : FullAuditedEntity<long>, IMayHaveTenant
    {
        //
        // 摘要:
        //     TenantId of this entity.
        public virtual int? TenantId { get; set; }

        public long? ParentId { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }


        public string MoudleName { get; set; }


        public string Description { get; set; }

        public int Order { get; set; }

    }
}
