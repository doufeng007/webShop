using Abp.Application.Navigation;
using ZCYX.FRMSCore;

namespace EmailServer
{
    public class EmailServerNavigationProvider : NavigationProvider
    {
        private readonly FRMSCoreNavigationProvider _baseProvider;
        public EmailServerNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "Email");


        }
    }
}
