using System.Collections.Generic;
using ZCYX.FRMSCore.Authorization.Permissions.Dto;

namespace ZCYX.FRMSCore.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}