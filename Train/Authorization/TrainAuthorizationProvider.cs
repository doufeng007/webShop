using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization;

namespace Train.Authorization
{
    public class TrainAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public TrainAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            _baseprovider.SetPermissionsWithMouldName(context, "Train");
        }

    }
}
