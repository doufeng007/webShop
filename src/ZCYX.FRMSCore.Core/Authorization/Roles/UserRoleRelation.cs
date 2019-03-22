using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.Authorization.Roles
{
    public class UserRoleRelation : UserRole
    {
        public virtual Guid? RelationId { get; set; }
    }
}
