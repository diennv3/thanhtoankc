using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Editions.Dto;
using Karion.BusinessSolution.MultiTenancy.Dto;

namespace Karion.BusinessSolution.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}