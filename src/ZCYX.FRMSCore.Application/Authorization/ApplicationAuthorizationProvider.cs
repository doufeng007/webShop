using Abp.Authorization;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Authorization.Permissions;
using System.Globalization;
using Abp.Dependency;

namespace ZCYX.FRMSCore.Authorization
{
    public class ApplicationAuthorizationProvider : AuthorizationProvider
    {
        private readonly IAbpPermissionBaseAppService _abpPermissionBaseAppService;
        public ApplicationAuthorizationProvider(IAbpPermissionBaseAppService abpPermissionBaseAppService)
        {
            _abpPermissionBaseAppService = abpPermissionBaseAppService;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {

            //var _permissionBaseAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IAbpPermissionBaseAppService>();
            var _permissionBaseAppService = _abpPermissionBaseAppService;
            var data = _permissionBaseAppService.GetByMoudleName("Application");
            var firstNodes = data.Where(r => !r.ParentId.HasValue);
            foreach (var item in firstNodes)
            {
                var permissionNode = context.GetPermissionOrNull(item.Code) ?? context.CreatePermission(item.Code, L(item.DisplayName));
                SetPermissionsWithDown(data, item, permissionNode);
            }


            //var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            ////var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));


            //var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"));
            //tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("TenantsCreate"));

            //var roles = pages.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            //roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("RolesCreate"));
            //roles.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangFireManager"));


            //var organizationUnits = pages.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            //organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_Create, L("CreatingNewOrganizationUnits"));
            //organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_Edit, L("EditingOrganizationUnits"));
            //organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_Delete, L("DeletingOrganizationUnits"));

            //var users = organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            //users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            //users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            //users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            //users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            //users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            //var realationSystem = organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_RealationSystem, L("RealationSystem"));
        }


        public void SetPermissionsWithDown(List<AbpPermissionBaseDto> source, AbpPermissionBaseDto permissionNode, Permission per)
        {
            var items = source.Where(r => r.ParentId == permissionNode.Id);
            if (items.Count() > 0)
            {

                foreach (var item in items)
                {
                    var node = per.CreateChildPermission(item.Code, L(item.DisplayName), null, Abp.MultiTenancy.MultiTenancySides.Host);
                    SetPermissionsWithDown(source, item, node);
                }
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }


        public void SetPermissionsWithMouldName(IPermissionDefinitionContext context, string mouldName)
        {
            var data = _abpPermissionBaseAppService.GetByMoudleName(mouldName);
            var firstNodes = data.Where(r => !r.ParentId.HasValue);
            foreach (var item in firstNodes)
            {
                var permissionNode = context.GetPermissionOrNull(item.Code) ?? context.CreatePermission(item.Code, L(item.DisplayName), null, Abp.MultiTenancy.MultiTenancySides.Host);
                SetPermissionsWithDown(data, item, permissionNode);
            }
        }
    }


}
