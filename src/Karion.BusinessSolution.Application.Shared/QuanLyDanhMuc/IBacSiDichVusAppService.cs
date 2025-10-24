using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IBacSiDichVusAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBacSiDichVuForViewDto>> GetAll(GetAllBacSiDichVusInput input);

        Task<GetBacSiDichVuForViewDto> GetBacSiDichVuForView(int id);

		Task<GetBacSiDichVuForEditOutput> GetBacSiDichVuForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBacSiDichVuDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBacSiDichVusToExcel(GetAllBacSiDichVusForExcelInput input);

		
		Task<PagedResultDto<BacSiDichVuUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BacSiDichVuDichVuLookupTableDto>> GetAllDichVuForLookupTable(GetAllForLookupTableInput input);
		
    }
}