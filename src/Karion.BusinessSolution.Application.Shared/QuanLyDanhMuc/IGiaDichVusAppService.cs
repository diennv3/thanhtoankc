using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IGiaDichVusAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGiaDichVuForViewDto>> GetAll(GetAllGiaDichVusInput input);

        Task<GetGiaDichVuForViewDto> GetGiaDichVuForView(int id);

		Task<GetGiaDichVuForEditOutput> GetGiaDichVuForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGiaDichVuDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGiaDichVusToExcel(GetAllGiaDichVusForExcelInput input);

		
		Task<PagedResultDto<GiaDichVuDichVuLookupTableDto>> GetAllDichVuForLookupTable(GetAllForLookupTableInput input);
		
    }
}