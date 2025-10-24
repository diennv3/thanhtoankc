using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution.Localization
{
    public static class BusinessSolutionLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BusinessSolutionConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(BusinessSolutionLocalizationConfigurer).GetAssembly(),
                        "Karion.BusinessSolution.Localization.BusinessSolution"
                    )
                )
            );
        }
    }
}