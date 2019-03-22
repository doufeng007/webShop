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

namespace HR
{
    [DependsOn(
     typeof(ZCYX.FRMSCore.FRMSCoreApplicationModule),
      typeof(Abp.WorkFlow.WorkFlowModule)
        , typeof(Abp.File.AbpFileModule))]
    public class HRModule : AbpModule
    {
        public override void PreInitialize()
        {
            //各种配置
            //为应用添加语言
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-Hans", "Chinese", "demo-language-css", true));
            //Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));

            ////添加本地化资源
            //Configuration.Localization.Sources.Add(
            //    new XmlLocalizationSource(
            //        "SimpleTaskSystem",
            //        HttpContext.Current.Server.MapPath("~/Localization/SimpleTaskSystem")
            //        )
            //    );
            //        Configuration.Localization.Sources.Add(
            //new DictionaryBasedLocalizationSource(
            //   "DemoLanguage",
            //    new XmlFileLocalizationDictionaryProvider(
            //        HttpContext.Current.Server.MapPath("~/Localization")
            //        )
            //    );

            /*
             对于内嵌的XML文件，我们应该将所有的本地化XML文件标记为内嵌的资源（选中xml文件，打开属性窗口，将生成操作的值改为‘内嵌的资源’）。然后像下面那样注册该本地化资源.
             XmlEmbeddedFileLocalizationDictionaryProvider会获得包含XML文件的程序集（GetExecutingAssembly简单地指向当前的程序集）和XML文件的 命名空间（程序集名称+xml文件的文件夹层次）。

注意：当给内嵌的XML文件起名字时，要加上语言后缀，但是不要使用“.”，比如“MySource.ch.xml”，而要使用短号“-”，比如“MySource-en.xml”。因为当寻找资源时，“.”会造成问题。
             */
            Configuration.Localization.Sources.Add(new DictionaryBasedLocalizationSource(HRConsts.LocalizationSourceName, new XmlEmbeddedFileLocalizationDictionaryProvider(
                 Assembly.GetExecutingAssembly(),
                 $"{HRConsts.LocalizationSourceName}.Localization")));
            Configuration.Authorization.Providers.Add<HRAuthorizationProvider>();

            //配置导航菜单
            Configuration.Navigation.Providers.Add<HRNavigationProvider>();

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HRModule).GetAssembly());

            //这里会自动去扫描程序集中配置好的映射关系
            //DapperExtensions.SetMappingAssemblies(new List<Assembly> { typeof(MyModule).GetAssembly() });

        }
    }
}
