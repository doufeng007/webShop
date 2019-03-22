using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Entity
{
    public class AbpPermissions: PermissionSetting
    {
        public int? RoleId { get; set; }
        public string Discriminator { get; set; }
        public long? UserId { get; set; }
    }
}
