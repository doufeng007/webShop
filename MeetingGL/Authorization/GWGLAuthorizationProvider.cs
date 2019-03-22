using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization;

namespace MeetingGL
{
    public class MeetingGLAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public MeetingGLAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            _baseprovider.SetPermissionsWithMouldName(context, "MeetingGL");
        }

    }
}
