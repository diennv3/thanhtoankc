using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using GetAllForLookupTableInput = Karion.BusinessSolution.QuanLyDanhMuc.Dtos.GetAllForLookupTableInput;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface ILichHenKhamsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLichHenKhamForViewDto>> GetAll(GetAllLichHenKhamsInput input);

        Task<GetLichHenKhamForViewDto> GetLichHenKhamForView(int id);

		Task<GetLichHenKhamForEditOutput> GetLichHenKhamForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLichHenKhamDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<LichHenKhamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<LichHenKhamNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<LichHenKhamNguoiThanLookupTableDto>> GetAllNguoiThanForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<LichHenKhamChuyenKhoaLookupTableDto>> GetAllChuyenKhoaForLookupTable(GetAllForLookupTableInput input);

        Task<int> ChuyenBenhNhan(ThongTinChuyenDto input);
        Task<List<LichHenKhamDto>> GetDetectedFacesHenKham();

		Task<PagedResultDto<DichVuChuyenKhoaVaGiaDichVuDto>> GetDichVuChuyenKhoaVaGiaDichVu(GetDichVuChuyenKhoaVaGiaDichVuInput chuyenKhoaId);

		Task<int> CapNhatDichVuLichHenKham(ThongTinCapNhatDichVuLichHenKhamDto input);

		Task<int> HoanTatThanhToan(int id);
		Task<int> ThanhToanVienPhi(ThanhToanVienPhiDto input);

		// string Generator(GeneratorQRDto input);
		string Generator(int lichHenKhamId, int chiTietThanhToanId);

		//comment api cũ
		//Task<int> CapNhatTienThua(ThongTinCapNhatTienThuaLichHenKhamDto input);
		Task<GetBaoCaoOutput> GetBaoCao(GetBaoCaoInput input);
		Task<GetKhungKhamResultDto> GetKhungKham(GetKhungKhamDto input);
    }
}