using Abp.Authorization;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Permissions;

namespace B_H5
{
    public class B_H5AuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public B_H5AuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            _baseprovider.SetPermissionsWithMouldName(context, "WebShop");
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, B_H5Consts.LocalizationSourceName);
        }
    }
}
