using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Supply
{
    public class SupplyNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public SupplyNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "Supply");

            

        }
        
    }
}
