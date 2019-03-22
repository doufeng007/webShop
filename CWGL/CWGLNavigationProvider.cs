using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace CWGL
{
    public class CWGLNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public CWGLNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        /// <summary>
        /// /7777
        /// </summary>
        /// <param name="context"></param>
        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "CWGL");


        }
    }
}
