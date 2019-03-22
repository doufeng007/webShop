
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace EmailServer
{
    [DependsOn(
    typeof(ZCYX.FRMSCore.FRMSCoreApplicationModule))]
    public class EmailServerModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EmailServerAuthorizationProvider>();

            //配置导航菜单
            Configuration.Navigation.Providers.Add<EmailServerNavigationProvider>();

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmailServerModule).GetAssembly());
        }
    }
}
