using Abp.Authorization;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization;

namespace Abp.WorkFlowDictionary.Authorization
{
    public class WorkFlowDictionaryAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public WorkFlowDictionaryAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //_baseprovider.SetPermissionsWithMouldName(context, "WorkFlowDictionary");

            //var dictionary = context.CreatePermission(AppPermissions.Pages_WorkFlowDictionary, L("WorkFlowDictionary"));
            //dictionary.CreateChildPermission(AppPermissions.Pages_WorkFlowDictionary_Create, L("WorkFlowDictionaryCreate"));
            //dictionary.CreateChildPermission(AppPermissions.Pages_WorkFlowDictionary_Edit, L("WorkFlowDictionaryEdit"));
            //dictionary.CreateChildPermission(AppPermissions.Pages_WorkFlowDictionary_Delete, L("WorkFlowDictionaryDelete"));
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
