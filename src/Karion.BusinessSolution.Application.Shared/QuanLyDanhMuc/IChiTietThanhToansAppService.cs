using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IChiTietThanhToansAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChiTietThanhToanForViewDto>> GetAll(GetAllChiTietThanhToansInput input);

        Task<GetChiTietThanhToanForViewDto> GetChiTietThanhToanForView(int id);

		Task<GetChiTietThanhToanForEditOutput> GetChiTietThanhToanForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditChiTietThanhToanDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetChiTietThanhToansToExcel(GetAllChiTietThanhToansForExcelInput input);

		
		Task<PagedResultDto<ChiTietThanhToanLichHenKhamLookupTableDto>> GetAllLichHenKhamForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ChiTietThanhToanNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		Task<MBBankJsonResponDto> MBBankGetTransactionHistory(MBBankGetTransactionHistoryDto input);
		Task KiemTraThanhToanNganHang(KiemTraThanhToanNganHangDto input);

    }
}