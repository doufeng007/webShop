using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using ZCYX.FRMSCore.Authentication.JwtBearer;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.EntityFrameworkCore;
using Abp.Runtime.Caching.Redis;
#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#endif

namespace ZCYX.FRMSCore
{
    [DependsOn(
         typeof(FRMSCoreApplicationModule),
         typeof(FRMSCoreEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
#if FEATURE_SIGNALR 
        ,typeof(AbpWebSignalRModule)
#endif
        , typeof(AbpRedisCacheModule)
     )]
    public class FRMSCoreWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FRMSCoreWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                FRMSCoreConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(FRMSCoreApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();


            var enableRedis = _appConfiguration.GetValue<bool>("Abp:RedisCache:Enable");
            if (enableRedis)
            {
                Configuration.Caching.UseRedis(options =>
                {
                    options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
                    options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
                });
            }




        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FRMSCoreWebCoreModule).GetAssembly());
        }
    }
}
