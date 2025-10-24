using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.HanetTenant
{
    public interface IHanetTenantLogsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHanetTenantLogForViewDto>> GetAll(GetAllHanetTenantLogsInput input);

        Task<GetHanetTenantLogForViewDto> GetHanetTenantLogForView(int id);

		Task<GetHanetTenantLogForEditOutput> GetHanetTenantLogForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditHanetTenantLogDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetHanetTenantLogsToExcel(GetAllHanetTenantLogsForExcelInput input);

		
    }
}