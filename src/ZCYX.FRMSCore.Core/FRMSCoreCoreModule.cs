using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Localization;
using ZCYX.FRMSCore.MultiTenancy;
using ZCYX.FRMSCore.Timing;

namespace ZCYX.FRMSCore
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class FRMSCoreCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            FRMSCoreLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = FRMSCoreConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FRMSCoreCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
