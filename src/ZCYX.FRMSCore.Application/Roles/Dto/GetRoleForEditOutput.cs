using Abp.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Authorization.Permissions.Dto;
using ZCYX.FRMSCore.Authorization.Roles;

namespace ZCYX.FRMSCore.Roles.Dto
{

    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }

    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}