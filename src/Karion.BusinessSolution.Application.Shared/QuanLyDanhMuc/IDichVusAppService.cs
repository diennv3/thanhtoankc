using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IDichVusAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDichVuForViewDto>> GetAll(GetAllDichVusInput input);

        Task<GetDichVuForViewDto> GetDichVuForView(int id);

		Task<GetDichVuForEditOutput> GetDichVuForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDichVuDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDichVusToExcel(GetAllDichVusForExcelInput input);

		
		Task<PagedResultDto<DichVuChuyenKhoaLookupTableDto>> GetAllChuyenKhoaForLookupTable(GetAllForLookupTableInput input);
		
    }
}