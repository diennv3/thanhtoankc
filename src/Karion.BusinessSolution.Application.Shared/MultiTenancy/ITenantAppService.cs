using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.MultiTenancy.Dto;

namespace Karion.BusinessSolution.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        Task<PagedResultDto<TenantListDtoCustom>> GetTenants(GetTenantsInput input);

        Task CreateTenant(CreateTenantInput input);

        Task<TenantEditDto> GetTenantForEdit(EntityDto input);

        Task UpdateTenant(TenantEditDto input);

        Task DeleteTenant(EntityDto input);

        Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input);

        Task UpdateTenantFeatures(UpdateTenantFeaturesInput input);

        Task ResetTenantSpecificFeatures(EntityDto input);

        Task UnlockTenantAdmin(EntityDto input);
    }
}
