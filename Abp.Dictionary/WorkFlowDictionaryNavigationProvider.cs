using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Abp.UI;
using ZCYX.FRMSCore;

namespace Abp.WorkFlowDictionary
{
    public class WorkFlowDictionaryNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public WorkFlowDictionaryNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "WorkFlowDictionary");


            ////获取【系统管理】菜单，并将当前菜单添加到【系统管理】菜单下
            //var administrator = context.Manager.MainMenu.Items.First(ite=>ite.Name== "Administration");
            //if (administrator == null) {
            //    throw new Exception("未找到【系统管理】菜单");
            //}
            //administrator.AddItem(new MenuItemDefinition(AppConsts.WorkFlowDictionary,L("WorkFlowDictionary"),icon: "ios-book", url: "/admin/dictionary",requiredPermissionName: AppPermissions.Pages_WorkFlowDictionary));
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
