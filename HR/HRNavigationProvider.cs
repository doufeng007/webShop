﻿using Abp.Application.Navigation;
using Abp.Localization;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Permissions;

namespace HR
{
    public class HRNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public HRNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "HR");
            



            //var administrator = context.Manager.MainMenu.Items.First(ite => ite.Name == "Administration");
            //if (administrator == null)
            //{
            //    throw new Exception("未找到【系统管理】菜单");
            //}
            //administrator.AddItem(new MenuItemDefinition(AppConsts.WorkFlow, L("WorkFlow"), icon: "ios-book", url: "/admin/flows"
            //    , requiredPermissionName: AppPermissions.Pages_WorkFlow
            //    ));

            //administrator.AddItem(new MenuItemDefinition(AppConsts.WorkFlowModels, L("WorkFlowModels"), icon: "egg", url: "/admin/models"
            //    , requiredPermissionName: AppPermissions.Pages_WorkFlowModels
            //    ));
        }


        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, HRConsts.LocalizationSourceName);
        }
    }
}
