using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Configuration.Tenants.Dto;

namespace Karion.BusinessSolution.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
