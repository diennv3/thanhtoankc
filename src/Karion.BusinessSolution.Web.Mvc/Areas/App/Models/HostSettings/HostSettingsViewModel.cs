using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Configuration.Host.Dto;
using Karion.BusinessSolution.Editions.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}