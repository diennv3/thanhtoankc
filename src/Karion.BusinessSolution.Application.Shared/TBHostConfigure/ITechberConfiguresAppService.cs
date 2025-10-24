using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.TBHostConfigure
{
    public interface ITechberConfiguresAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTechberConfigureForViewDto>> GetAll(GetAllTechberConfiguresInput input);

        Task<GetTechberConfigureForViewDto> GetTechberConfigureForView(int id);

		Task<GetTechberConfigureForEditOutput> GetTechberConfigureForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTechberConfigureDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTechberConfiguresToExcel(GetAllTechberConfiguresForExcelInput input);

		
    }
}