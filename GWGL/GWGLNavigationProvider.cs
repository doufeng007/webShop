using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace GWGL
{
    public class GWGLNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public GWGLNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "GWGL");


        }
    }
}
