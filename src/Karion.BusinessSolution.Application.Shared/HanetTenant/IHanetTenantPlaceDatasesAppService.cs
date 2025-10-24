using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.HanetTenant
{
    public interface IHanetTenantPlaceDatasesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHanetTenantPlaceDatasForViewDto>> GetAll(GetAllHanetTenantPlaceDatasesInput input);

        Task<GetHanetTenantPlaceDatasForViewDto> GetHanetTenantPlaceDatasForView(int id);

		Task<GetHanetTenantPlaceDatasForEditOutput> GetHanetTenantPlaceDatasForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditHanetTenantPlaceDatasDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetHanetTenantPlaceDatasesToExcel(GetAllHanetTenantPlaceDatasesForExcelInput input);

		
    }
}