using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ZCYX.FRMSCore.Localization
{
    public static class FRMSCoreLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(FRMSCoreConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(FRMSCoreLocalizationConfigurer).GetAssembly(),
                        "ZCYX.FRMSCore.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
