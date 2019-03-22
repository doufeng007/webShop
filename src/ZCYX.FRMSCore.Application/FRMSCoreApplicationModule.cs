using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Reflection;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Permissions;

namespace ZCYX.FRMSCore
{
    [DependsOn(
        typeof(FRMSCoreCoreModule), 
        typeof(AbpAutoMapperModule),
        typeof(Abp.IM.AbpIMModule))]
    public class FRMSCoreApplicationModule : AbpModule
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public override void PreInitialize()
        {
            //IocManager.RegisterIfNot<IAbpPermissionBaseAppService, AbpPermissionBaseAppService>(DependencyLifeStyle.Transient);
            //添加语言
            //Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-Hans", "中文", "famfamfam-flag-tr"));
            Configuration.Localization.ReturnGivenTextIfNotFound = true;
            Configuration.Localization.WrapGivenTextIfNotFound = false;


            /*
 对于内嵌的XML文件，我们应该将所有的本地化XML文件标记为内嵌的资源（选中xml文件，打开属性窗口，将生成操作的值改为‘内嵌的资源’）。然后像下面那样注册该本地化资源.
 XmlEmbeddedFileLocalizationDictionaryProvider会获得包含XML文件的程序集（GetExecutingAssembly简单地指向当前的程序集）和XML文件的 命名空间（程序集名称+xml文件的文件夹层次）。

注意：当给内嵌的XML文件起名字时，要加上语言后缀，但是不要使用“.”，比如“MySource.ch.xml”，而要使用短号“-”，比如“MySource-en.xml”。因为当寻找资源时，“.”会造成问题。
 */
            Configuration.Localization.Sources.Add(new DictionaryBasedLocalizationSource(AppConsts.LocalizationSourceName, new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                $"{AppConsts.LocalizationSourceName}.Localization")));
            Configuration.Authorization.Providers.Add<ApplicationAuthorizationProvider>();
            //配置导航菜单
            Configuration.Navigation.Providers.Add<FRMSCoreNavigationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(FRMSCoreApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg.AddProfiles(thisAssembly);
            });
        }
    }
}
