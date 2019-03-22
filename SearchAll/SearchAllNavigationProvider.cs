using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace SearchAll
{
    public class SearchAllNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public SearchAllNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "SearchAll");


        }
    }
}
