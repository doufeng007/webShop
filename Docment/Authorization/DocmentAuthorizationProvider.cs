using Abp.Authorization;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization;

namespace Docment.Authorization
{
    
        public class DocmentAuthorizationProvider : AuthorizationProvider
        {
            private readonly ApplicationAuthorizationProvider _baseprovider;
            public DocmentAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
            {
                _baseprovider = baseprovider;
            }

            public override void SetPermissions(IPermissionDefinitionContext context)
            {
                _baseprovider.SetPermissionsWithMouldName(context, "Docment");
            }
            
        }
    
}
