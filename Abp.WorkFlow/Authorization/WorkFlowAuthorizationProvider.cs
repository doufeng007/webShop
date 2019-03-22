using Abp.Authorization;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Permissions;

namespace Abp.WorkFlow.Authorization
{
    public class WorkFlowAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public WorkFlowAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //var workFlower = context.CreatePermission(AppPermissions.Pages_WorkFlow, L("WorkFlow"));
            //var workFlowerModels = context.CreatePermission(AppPermissions.Pages_WorkFlowModels, L("WorkFlowModels"));


           // _baseprovider.SetPermissionsWithMouldName(context, "WorkFlow");
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
