using Abp.Application.Navigation;
using Abp.Localization;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Permissions;

namespace Train
{
    public class TrainNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public TrainNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        /// <summary>
        /// /7777
        /// </summary>
        /// <param name="context"></param>
        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "Train");


        }
    }
}
