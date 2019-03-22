using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using Abp.AspNetCore.Configuration;
using Abp.File;
using Abp.SignalR.Core;
using GWGL;
using Abp.AspNetCore.SignalR;
using Train;
using IMLib;
using Abp.Excel;
using TaskGL;

namespace ZCYX.FRMSCore.Web.Host.Startup
{
    [DependsOn(
       typeof(FRMSCoreWebCoreModule), typeof(Abp.WorkFlow.WorkFlowModule), typeof(Abp.WorkFlowDictionary.WorkFlowDictionaryModule), typeof(AbpExcelModule)
        , typeof(EmailServer.EmailServerModule)
        , typeof(Project.ProjectModule)
        , typeof(AbpAspNetCoreSignalRModule)
        , typeof(AbpFileModule)
        , typeof(AbpSignalRCoreModule)
        , typeof(HR.HRModule)
        , typeof(Docment.DocmentModule)
        , typeof(XZGL.XZGLModule)
        , typeof(SearchAll.SearchAllModule)
        , typeof(CWGL.CWGLModule)
        , typeof(Supply.SupplyModule)
        , typeof(GWGL.GWGLModule)
        , typeof(GWGL.GWGLModule)
        , typeof(IMLib.IMLibModule)
        , typeof(HQGL.HQGLModule)
        , typeof(HQGL.HQGLModule)
        , typeof(MeetingGL.MeetingGLModule)
        , typeof(TaskGLModule)
        , typeof(TrainModule))]
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
            typeof(Project.ProjectModule).GetAssembly()
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
        typeof(Supply.SupplyModule).GetAssembly()
        );

            Configuration.Modules.AbpAspNetCore()
       .CreateControllersForAppServices(
       typeof(Docment.DocmentModule).GetAssembly()
       );
            Configuration.Modules.AbpAspNetCore()
        .CreateControllersForAppServices(
        typeof(GWGLModule).GetAssembly()
        );

            Configuration.Modules.AbpAspNetCore()
      .CreateControllersForAppServices(
      typeof(XZGL.XZGLModule).GetAssembly()
      );
      Configuration.Modules.AbpAspNetCore()
      .CreateControllersForAppServices(
      typeof(CWGL.CWGLModule).GetAssembly()
      );

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(EmailServer.EmailServerModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(TrainModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(IMLibModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(HQGL.HQGLModule).GetAssembly()
                );

            Configuration.Modules.AbpAspNetCore()
              .CreateControllersForAppServices(
                  typeof(MeetingGL.MeetingGLModule).GetAssembly()
              );

            Configuration.Modules.AbpAspNetCore()
              .CreateControllersForAppServices(
                  typeof(TaskGLModule).GetAssembly()
              );
        }
    }
}
