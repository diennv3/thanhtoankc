using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.HanetTenant
{
    public interface IHanetTenantDeviceDatasesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHanetTenantDeviceDatasForViewDto>> GetAll(GetAllHanetTenantDeviceDatasesInput input);

        Task<GetHanetTenantDeviceDatasForViewDto> GetHanetTenantDeviceDatasForView(int id);

		Task<GetHanetTenantDeviceDatasForEditOutput> GetHanetTenantDeviceDatasForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditHanetTenantDeviceDatasDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetHanetTenantDeviceDatasesToExcel(GetAllHanetTenantDeviceDatasesForExcelInput input);

		
		Task<PagedResultDto<HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableDto>> GetAllHanetTenantPlaceDatasForLookupTable(GetAllForLookupTableInput input);
		
    }
}