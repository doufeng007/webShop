using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using Abp.AspNetCore.Configuration;
using Abp.File;
using Abp.SignalR.Core;
using Abp.AspNetCore.SignalR;
using IMLib;
using Abp.Excel;
using Abp.WeChat;

namespace ZCYX.FRMSCore.Web.Host.Startup
{
    [DependsOn(
       typeof(FRMSCoreWebCoreModule), typeof(Abp.WorkFlow.WorkFlowModule), typeof(Abp.WorkFlowDictionary.WorkFlowDictionaryModule), typeof(AbpExcelModule)
        , typeof(EmailServer.EmailServerModule)
        , typeof(AbpAspNetCoreSignalRModule)
        , typeof(AbpFileModule)
        , typeof(AbpSignalRCoreModule)
        , typeof(AbpWeChatModule)
        , typeof(HR.HRModule)
        , typeof(SearchAll.SearchAllModule)
        , typeof(IMLib.IMLibModule)
        , typeof(B_H5.B_H5Module))]
    //[DependsOn(
    //   typeof(FRMSCoreWebCoreModule))]
    public class FRMSCoreWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FRMSCoreWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FRMSCoreWebHostModule).GetAssembly());

            Configuration.Modules.AbpAspNetCore()
              .CreateControllersForAppServices(
              typeof(Abp.WorkFlow.WorkFlowModule).GetAssembly()
              );

            Configuration.Modules.AbpAspNetCore()
             .CreateControllersForAppServices(
             typeof(Abp.WorkFlowDictionary.WorkFlowDictionaryModule).GetAssembly()
             );


            Configuration.Modules.AbpAspNetCore()
            .CreateControllersForAppServices(
            typeof(AbpFileModule).GetAssembly()
            );


            Configuration.Modules.AbpAspNetCore()
            .CreateControllersForAppServices(
            typeof(AbpSignalRCoreModule).GetAssembly()
            );

            Configuration.Modules.AbpAspNetCore()
          .CreateControllersForAppServices(
          typeof(HR.HRModule).GetAssembly()
          );


            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(EmailServer.EmailServerModule).GetAssembly()
                );


            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(IMLibModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(AbpWeChatModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
              .CreateControllersForAppServices(
                  typeof(B_H5.B_H5Module).GetAssembly()
              );


        }
    }
}
