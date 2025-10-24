using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Authorization.Users.Profile.Dto;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

namespace Karion.BusinessSolution.MobileAppServices
{
    public interface IMobileAppServices : IApplicationService
    {
        Task<MobileDto.MobileLoginResult> MobileLogin(MobileDto.MobileLoginModel model);
        Task<MobileDto.GetTenantsResultDto> GetTenants(MobileDto.GetMenuDto input);
        Task<MobileDto.GetChuyenKhoaResultDto> GetChuyenKhoa(MobileDto.GetChuyenKhoaDto input);
        Task<MobileDto.GetDichVuResultDto> GetDichVu(MobileDto.GetChuyenKhoaDto input);
        Task<MobileDto.GetThongTinChuyenKhoaResultDto> GetThongTinChuyenKhoa(MobileDto.GetThongTinChuyenKhoaDto input);
        Task<MobileDto.GetThongTinDichVuResultDto> GetThongTinDichVu(MobileDto.GetThongTinDichVuDto input);
        Task<String> Verify(string code);
        Task<MobileDto.CommonResponse> Register(MobileDto.MobileRegisterDto input);
        Task<MobileDto.CommonResponse> DangKyNguoiThan(MobileDto.DangKyNguoiThanDto input);
        Task<MobileDto.GetDanhSachNguoiThanDto> GetDanhSachNguoiThan(MobileDto.GetMenuDto input);
        Task<MobileDto.CommonResponse> DangKyKham(MobileDto.DangKyKhamDto input);
        Task<MobileDto.GetDanhSachDangKyKhamResultDto> GetDanhSachDangKyKham(MobileDto.GetDanhSachDangKyKhamDto input);
        string PaymentQRGenerator(MobileDto.QRInputDto input);
        Task<MobileDto.CommonResponse> DoiMatKhau(MobileDto.DoiMatKhauDto input);
        Task<MobileDto.KetQuaTraveThongTinNguoiBenhResultDto> GetThongTinNguoiBenh(MobileDto.GetThongTinNguoiBenh input);
        Task<MobileDto.CommonResponse> CapNhatThongTinNguoiBenh(MobileDto.CapNhatThongTinNguoiBenhDto input);

        Task<MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto> GetDanhSachBacSiChuyenKhoaTenant(MobileDto.GetDanhSachBacSiChuyenKhoaTenantDto input);

        Task<MobileDto.CommonResponse> LuuDeviceToken(MobileDto.CapNhatThongTinDeviceToken input);
        Task<MobileDto.AvatarChangeResultDto> UpdateAnh(MobileDto.AvatarChangeDto input);
        Task<MobileDto.GetThongBaoNguoiBenhResultDto> GetThongBaoNguoiBenh(MobileDto.GetMenuDto input);
        Task<MobileDto.CommonResponse> SetTrangThaiThongBaoNguoiBenh(MobileDto.SetTrangThaiThongBaoNguoiBenhDto input);
        Task<MobileDto.GetGioKhamTheoKhungResultDto> GetGioKhamTheoKhung(MobileDto.GetGioKhamTheoKhungDto input);
        Task<MobileDto.CommonResponse> EditLichHenKham(MobileDto.EditLichHenKhamDto input);
        Task<MobileDto.CommonResponse> HuyLichHenKham(MobileDto.HuyLichHenKhamDto input);
        Task<MobileDto.GetImageBacSiOutput> GetProfilePictureById(MobileDto.GetImageBacSi input);
        Task<MobileDto.GetDanhSachNhanVienResultDto> GetAllNhanVien(MobileDto.GetMenuDto input);
        
        Task<MobileDto.CommonResponse> DiemDanhNhanVien(MobileDto.DiemDanhInputDto input);
        Task<MobileDto.GetLichSuDiemDanhResultDto> GetLichSuDiemDanhById(MobileDto.GetLichSuDiemDanhDto input);
        Task<MobileDto.ThongKeDiemDanhResultDto> BaoCaoDiemDanh(MobileDto.ThongKeDiemDanhInputDto input);

        Task<MobileDto.CommonResponse> UpdateDeviceToken(MobileDto.UpdateDeviceTokenDto input);
        Task<MobileDto.CommonResponse> DeleteDeviceToken(MobileDto.GetMenuDto input);
    }
}