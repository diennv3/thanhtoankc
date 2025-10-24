using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.VersionControl
{
    public interface IDanhSachVersionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDanhSachVersionForViewDto>> GetAll(GetAllDanhSachVersionsInput input);

        Task<GetDanhSachVersionForViewDto> GetDanhSachVersionForView(int id);

		Task<GetDanhSachVersionForEditOutput> GetDanhSachVersionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDanhSachVersionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDanhSachVersionsToExcel(GetAllDanhSachVersionsForExcelInput input);

		
    }
}