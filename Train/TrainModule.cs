using Abp.Dapper;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Train.Authorization;

namespace Train
{
    [DependsOn(
     typeof(ZCYX.FRMSCore.FRMSCoreApplicationModule),
      typeof(Abp.WorkFlow.WorkFlowModule)
        , typeof(Abp.File.AbpFileModule))]
    public class TrainModule : AbpModule
    {
        public override void PreInitialize()
        {

            Configuration.Caching.Configure(CourseStatisticsAppService.StatisiticCacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(CourseStatisticsAppService.StatisiticCacheExpiryTime);
            });
            Configuration.Authorization.Providers.Add<TrainAuthorizationProvider>();

            //配置导航菜单
            Configuration.Navigation.Providers.Add<TrainNavigationProvider>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TrainModule).GetAssembly());

            //这里会自动去扫描程序集中配置好的映射关系
            //DapperExtensions.SetMappingAssemblies(new List<Assembly> { typeof(MyModule).GetAssembly() });

        }
    }
}
