using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using ZCYX.FRMSCore.Authorization;

namespace Supply
{
    public class SupplyAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public SupplyAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }
        public override void SetPermissions(IPermissionDefinitionContext context)
        {

            _baseprovider.SetPermissionsWithMouldName(context, "Supply");
        }

       
    }
}
