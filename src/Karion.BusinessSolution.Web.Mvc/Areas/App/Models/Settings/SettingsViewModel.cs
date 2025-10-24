using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Configuration.Tenants.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}