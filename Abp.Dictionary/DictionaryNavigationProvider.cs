using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlowDictionary
{
    public class DictionaryNavigationProvider : NavigationProvider
    {
        public const string LocalizationSourceName = "WorkFlowLanguage";
        public const string MenuName = "WorkFlowMenu";
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));
            menu.AddItem(new MenuItemDefinition("WorkFlowMenu", L("WorkFlowMenuName"), icon: "icon-lock"));
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LocalizationSourceName);
        }
    }
}
