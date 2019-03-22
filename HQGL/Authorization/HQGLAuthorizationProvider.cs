using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization;

namespace HQGL.Authorization
{
    public class HQGLAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public HQGLAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            _baseprovider.SetPermissionsWithMouldName(context, "HQGL");
        }

    }
}
