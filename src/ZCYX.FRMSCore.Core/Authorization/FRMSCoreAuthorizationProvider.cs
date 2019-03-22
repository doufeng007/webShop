using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ZCYX.FRMSCore.Authorization
{
    public class FRMSCoreAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
           var userole= context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            //userole.CreateChildPermission(PermissionNames.Pages_Users_Test,L("Test")).CreateChildPermission(PermissionNames.Pages_Users_Test_SubText,L("SubText"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, FRMSCoreConsts.LocalizationSourceName);
        }
    }
}
