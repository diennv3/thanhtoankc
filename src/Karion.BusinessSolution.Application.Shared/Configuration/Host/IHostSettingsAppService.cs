using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Configuration.Host.Dto;

namespace Karion.BusinessSolution.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
