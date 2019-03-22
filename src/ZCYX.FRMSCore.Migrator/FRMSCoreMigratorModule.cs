using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.EntityFrameworkCore;
using ZCYX.FRMSCore.Migrator.DependencyInjection;
using System;

namespace ZCYX.FRMSCore.Migrator
{
    [DependsOn(typeof(FRMSCoreEntityFrameworkModule))]
    public class FRMSCoreMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public FRMSCoreMigratorModule(FRMSCoreEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(FRMSCoreMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                FRMSCoreConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
            AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FRMSCoreMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
