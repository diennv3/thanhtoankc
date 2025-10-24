using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.NguoiBenhTestNS.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.NguoiBenhTestNS
{
    public interface INguoiBenhTestsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNguoiBenhTestForViewDto>> GetAll(GetAllNguoiBenhTestsInput input);

        Task<GetNguoiBenhTestForViewDto> GetNguoiBenhTestForView(int id);

		Task<GetNguoiBenhTestForEditOutput> GetNguoiBenhTestForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditNguoiBenhTestDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetNguoiBenhTestsToExcel(GetAllNguoiBenhTestsForExcelInput input);

		
    }
}