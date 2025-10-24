using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.VersionControl
{
    public interface IVersionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetVersionForViewDto>> GetAll(GetAllVersionsInput input);

        Task<GetVersionForViewDto> GetVersionForView(int id);

		Task<GetVersionForEditOutput> GetVersionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditVersionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetVersionsToExcel(GetAllVersionsForExcelInput input);

		
    }
}