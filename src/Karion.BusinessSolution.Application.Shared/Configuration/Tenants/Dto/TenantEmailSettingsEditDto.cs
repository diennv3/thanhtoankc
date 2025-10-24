using Abp.Auditing;
using Karion.BusinessSolution.Configuration.Dto;

namespace Karion.BusinessSolution.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}