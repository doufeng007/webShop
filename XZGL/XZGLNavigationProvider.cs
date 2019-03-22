using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace XZGL
{
    public class XZGLNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public XZGLNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        /// <summary>
        /// /7777
        /// </summary>
        /// <param name="context"></param>
        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "XZGL");


        }
    }
}
