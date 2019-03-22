using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Permissions;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore
{
    public class FRMSCoreNavigationProvider : NavigationProvider
    {

        private readonly IAbpMenuBaseAppService _abpMenuBaseAppService;
        public FRMSCoreNavigationProvider(IAbpMenuBaseAppService abpMenuBaseAppService)
        {
            _abpMenuBaseAppService = abpMenuBaseAppService;
        }
        public override void SetNavigation(INavigationProviderContext context)
        {

            //var _menuBaseAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IAbpMenuBaseAppService>();
            var menus = _abpMenuBaseAppService.GetMenuFromCache();

            var moudleMenus = menus.Where(r => r.MoudleName == "Application");




            foreach (var item in moudleMenus)
            {
                if (!item.ParentId.HasValue)
                {
                    var oneMenu = new MenuItemDefinition(item.Code, L(item.DisplayName),
                      item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                      null, item.Target, item.IsEnabled, item.IsVisible, null);
                    context.Manager.MainMenu.AddItem(oneMenu);

                }
                else
                {
                    var parentMenuData = menus.FirstOrDefault(r => r.Id == item.ParentId);
                    if (parentMenuData == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "父菜单不存在");
                    var allMenuItems = new List<MenuItemDefinition>();
                    GetItemMenuByCodeDiGui(allMenuItems, context.Manager.MainMenu.Items.ToList());
                    // var parentMenu = context.Manager.MainMenu.Items.FirstOrDefault(ite => ite.Name == parentMenuData.Code);
                    var parentMenu = allMenuItems.FirstOrDefault(ite => ite.Name == parentMenuData.Code);
                    if (parentMenu == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到父菜单");
                    var oneMenu = new MenuItemDefinition(item.Code, L(item.DisplayName),
                        item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                        null, item.Target, item.IsEnabled, item.IsVisible, null);
                    parentMenu.AddItem(oneMenu);
                }


            }

            //context.Manager.MainMenu.AddItem(new MenuItemDefinition(
            //       AppConsts.App.Tenant.Dashboard,
            //       L("Home"),
            //       url: "/home",
            //       icon: "stats-bars",
            //       order: 0
            //       )
            //   );
            //context.Manager.MainMenu.AddItem(new MenuItemDefinition(AppConsts.App.Common.Administration, L("Administration"), icon: "stats-bars", order: 99)
            //   //.AddItem(new MenuItemDefinition(
            //   //    AppConsts.App.Host.Tenants,
            //   //    L("Tenants"),
            //   //    url: "/admin/tenants",
            //   //    icon: "person-stalker"
            //   //    , requiredPermissionName: AppPermissions.Pages_Tenants
            //   //    )
            //   //)
            //   .AddItem(new MenuItemDefinition(
            //       AppConsts.App.Common.Roles,
            //       L("Roles"),
            //       url: "/admin/role",
            //       icon: "person-stalker"
            //       , requiredPermissionName: AppPermissions.Pages_Administration_Roles
            //       )
            //   )
            //   .AddItem(new MenuItemDefinition(
            //       AppConsts.App.Common.OrganizationUnits,
            //       L("OrganizationUnits"),
            //       url: "/admin/organize",
            //       icon: "android-people"
            //       , requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
            //       )
            //   )
            //   .AddItem(new MenuItemDefinition(
            //       AppConsts.App.Common.RealationSystem,
            //       L("RealationSystem"),
            //       url: "/admin/realationsystem",
            //       icon: "android-people"
            //       , requiredPermissionName: AppPermissions.Pages_Administration_RealationSystem
            //       )
            //   )

            //   //.AddItem(new MenuItemDefinition(
            //   //    AppConsts.App.Common.AuditLogs,
            //   //    L("AuditLogs"),
            //   //    icon: "person-stalker"
            //   //    //, requiredPermissionName: AppPermissions.Pages_Administration_Roles
            //   //    ).AddItem(new MenuItemDefinition(AppConsts.App.Common.SystemLogs, L("SystemLogs"), url: "ff", icon: "dd"))
            //   //)
            //   );
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }

        public void SetMenusWithDown(List<AbpMenuBaseDto> source, AbpMenuBaseDto menuNode, MenuItemDefinition menu)
        {
            var items = source.Where(r => r.ParentId == menuNode.Id);
            if (items.Count() > 0)
            {
                foreach (var item in items)
                {
                    var node = menu.AddItem(new MenuItemDefinition(item.Code, L(item.DisplayName),
                        item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                        null, item.Target, item.IsEnabled, item.IsVisible, null)
                    { });
                    SetMenusWithDown(source, item, node);
                }
            }
        }


        public void SetNavigationWithMouldName(INavigationProviderContext context, string mouldName)
        {
            var menus = _abpMenuBaseAppService.GetMenuFromCache();

             var moudleMenus = menus.Where(r => r.MoudleName == mouldName).OrderBy(x=>x.Code);


            foreach (var item in moudleMenus)   
            {
                if (!item.ParentId.HasValue)
                {
                    var oneMenu = new MenuItemDefinition(item.Code, L(item.DisplayName),
                      item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                      null, item.Target, item.IsEnabled, item.IsVisible, null);
                    context.Manager.MainMenu.AddItem(oneMenu);

                }
                else
                {
                    var parentMenuData = menus.FirstOrDefault(r => r.Id == item.ParentId);
                    if (parentMenuData == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "父菜单不存在");
                    var allMenuItems = new List<MenuItemDefinition>();
                    GetItemMenuByCodeDiGui(allMenuItems,context.Manager.MainMenu.Items.ToList());
                    // var parentMenu = context.Manager.MainMenu.Items.FirstOrDefault(ite => ite.Name == parentMenuData.Code);
                    var parentMenu = allMenuItems.FirstOrDefault(ite => ite.Name == parentMenuData.Code);
                    if (parentMenu == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到父菜单");
                    var oneMenu = new MenuItemDefinition(item.Code, L(item.DisplayName),
                        item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                        null, item.Target, item.IsEnabled, item.IsVisible, null);
                    parentMenu.AddItem(oneMenu);
                }


            }
        }


    


        public void GetItemMenuByCodeDiGui(List<MenuItemDefinition> item, List<MenuItemDefinition> sourceItem)
        {

            foreach (var entity in sourceItem)
            {
                item.Add(entity);
                if (entity.Items.Count > 0)
                {
                    GetItemMenuByCodeDiGui(item, entity.Items.ToList());
                }
                

            }
        }
    }
}

