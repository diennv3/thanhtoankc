using System.Collections.Generic;
using Abp.Localization;
using Karion.BusinessSolution.Install.Dto;

namespace Karion.BusinessSolution.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
