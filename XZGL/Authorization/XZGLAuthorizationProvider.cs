using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization;

namespace XZGL.Authorization
{
    public class XZGLAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public XZGLAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            _baseprovider.SetPermissionsWithMouldName(context, "XZGL");
        }

    }
}
