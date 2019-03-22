using Abp.Authorization;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
