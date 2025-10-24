using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Karion.BusinessSolution.Authorization.Users.Delegation.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.QuanLyDiemDanh;

namespace Karion.BusinessSolution.Dto
{
    public class MobileDto
    {
        
        public class MobileLoginModel
        {
            public string AccountName { get; set; }
            public string PassWord { get; set; }
        }

        public class Version
        {
            public string type { get; set; }
            public int version { get; set; }
        }

        public class AppVersion
        {
            public List<Version> appVersions { get; set; }
        }

        public class VersionDto:CommonResponse
        {
            public List<Version> appVersions { get; set; }
        }

        public class CommonResponse
        {
            public bool status { get; set; }
            public int errorCode { get; set; }
            public string message { get; set; }
        }

        public class CheckTokenDto : CommonResponse
        {
            public int userid { get; set; }
        }

        public class MobileLoginResult : CommonResponse
        {
            public string token { get; set; }
            public string username { get; set; }
            public int userid { get; set; }
            public bool? IsNhanVien { get; set; }
        }

        public class MobileResult
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public class GetMenuDto
        {
            public int userid { get; set; }
            public string token { get; set; }
            public string username { get; set; }
        }

        public class MobileRegisterDto
        {
            [Required] [StringLength(250)] public string hoVaTen { get; set; }

            // [Required]
            // public string tuoi { get; set;}
            [Required] [StringLength(10)] public string gioiTinh { get; set; }
            [Required] [StringLength(1000)] public string diaChi { get; set; }
            [Required] [StringLength(20)] public string userName { get; set; }
            [Required] [StringLength(15)] public string phoneNumber { get; set; }
            public string emailAddress { get; set; }
            [Required] public string password { get; set; }
            [Required] public int ngaySinh { get; set; }
            [Required] public int thangSinh { get; set; }
            [Required] public int namSinh { get; set; }
            public string SoTheBHYT { get; set; }
            public string NoiDkBanDau { get; set; }
            public DateTime GiaTriSuDungTuNgay { get; set; }
        }

        public class TenantDto : GetMenuDto
        {
            public int tenantId { get; set; }
            public string tenantName { get; set; }
        }

        public class AvatarChangeDto : GetMenuDto
        {
            public byte[] Data { get; set; }
            public string JpegFileName { get; set; }
        }

        public class AvatarChangeResultDto : CommonResponse
        {
            public string ImageUrl { get; set; }
        }

        public class GetTenantsResultDto : CommonResponse
        {
            public List<TenantDto> tenants { get; set; }
        }

        public class GetDanhSachNguoiThanDto : CommonResponse
        {
            public List<NguoiThanDto> nguoiThan { get; set; }
        }

        public class GetDanhSachDangKyKhamDto : GetMenuDto
        {
            public DateTime? tuNgay { get; set; }
            public DateTime? denNgay { get; set; }
            public int? tenantId { get; set; }
        }

        // public class LichHenKhamContainer
        // {
        //     public LichHenKhamDto lichHenKham { get; set; }
        //     public ChuyenKhoaDto thongTinChuyenKhoa { get; set; }
        //     public DichVuDto thongTinDichVu { get; set; }
        //     public ThongTinBacSiDto thongTinBacSi { get; set; }
        //     public string type { get; set; }
        //     public TenantDto donViKham { get; set; }
        // }

        public class LichHenKhamContainer
        {
            //Lịch hẹn khám
            public DateTime NgayHenKham { get; set; }
            public string MoTaTrieuChung { get; set; }
            public bool IsCoBHYT { get; set; }
            public string SoTheBHYT { get; set; }
            public string NoiDangKyKCBDauTien { get; set; }
            public DateTime? BHYTValidDate { get; set; }
            public int PhuongThucThanhToan { get; set; }
            public bool IsDaKham { get; set; }
            public bool IsDaThanhToan { get; set; }
            public DateTime? TimeHoanThanhKham { get; set; }
            public DateTime? TimeHoanThanhThanhToan { get; set; }
            public string ChiDinhDichVuSerialize { get; set; }
            public int Flag { get; set; }
            public string QRString { get; set; }
            public long? BacSiId { get; set; }
            public decimal TongTienDaThanhToan { get; set; }
            public decimal TienThua { get; set; }
            public long? ThuNganId { get; set; }
            public int? NguoiBenhId { get; set; }
            public int? NguoiThanId { get; set; }

            public int? ChuyenKhoaId { get; set; }

            //Chuyên khoa
            public string TenChuyenKhoa { get; set; }

            public string MoTaChuyenKhoa { get; set; }

            //Tenant
            public int TenantId { get; set; }

            public string TenantName { get; set; }

            //bác sĩ
            public string Name { get; set; }
            public string Surname { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string TieuSu { get; set; }
            public string ChucDanh { get; set; }
            public string Type { get; set; }
            public int LichHenKhamId { get; set; }
            public int? KhungKham { get; set; }
            public string GioKham { get; set; }
        }

        public class GetDanhSachDangKyKhamResultDto : CommonResponse
        {
            public List<LichHenKhamContainer> lichHenKhamList { get; set; }
        }
        //get by tenants

        //CommonDto
        public class ThongTinBacSiDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public long Id { get; set; }
            public string TieuSu { get; set; }
            public string ChucDanh { get; set; }
        }

        public class DangKyNguoiThanDto : GetMenuDto
        {
            public string hoVaTen { get; set; }
            public int tuoi { get; set; }
            public string gioiTinh { get; set; }
            public string diaChi { get; set; }
            public string moiQuanHe { get; set; }
            public string soDienThoai { get; set; }
        }

        //Input
        public class GetChuyenKhoaDto : GetMenuDto
        {
            public int tenantId { get; set; }
        }

        public class GetThongTinChuyenKhoaDto : GetChuyenKhoaDto
        {
            public int chuyenKhoaId { get; set; }
        }

        public class GetThongTinDichVuDto : GetChuyenKhoaDto
        {
            public int dichVuId { get; set; }
        }

        public class DangKyKhamDto : GetChuyenKhoaDto
        {
            public int? nguoiThanId { get; set; }
            public long? bacSiId { get; set; }
            public DateTime ngayHenKham { get; set; }
            public string moTaTrieuChung { get; set; }
            public int? chuyenKhoaId { get; set; }
            public bool IsCoBHYT { get; set; }
            public string SoTheBHYT { get; set; }
            public string NoiDangKyKCBDauTien { get; set; }
            public int? KhungKham { get; set; }
        }

        //Output
        public class GetChuyenKhoaResultDto : CommonResponse
        {
            public List<ChuyenKhoaDto> ChuyenKhoa { get; set; }
        }

        public class GetDichVuResultDto : CommonResponse
        {
            public List<DichVuDto> DichVu { get; set; }
        }

        public class GetThongTinChuyenKhoaResultDto : CommonResponse
        {
            public ChuyenKhoaDto data { get; set; }
            public List<ThongTinBacSiDto> danhSachBacSi { get; set; }
        }

        public class GetThongTinDichVuResultDto : CommonResponse
        {
            public DichVuDto data { get; set; }
            public List<ThongTinBacSiDto> danhSachBacSi { get; set; }
            public GiaDichVuDto GiaDichVu { get; set; }
        }

        //QR
        public class QRInputDto
        {
            public int amount { get; set; }
            public string bankAccount { get; set; }
            public string bankCode { get; set; }
            public string transactionUid { get; set; }
            public string noiDung { get; set; }
        }

        public class QRStructure
        {
            public int id { get; set; }


            public string data { get; set; }

            public int dataLenght()
            {
                return data.Length;
            }

            public string dataString()
            {
                return id.ToString("D2") + dataLenght().ToString("D2") + data;
            }
        }

        public class DoiMatKhauDto : GetMenuDto
        {
            public string OldPassWord { get; set; }
            public string NewPassWord { get; set; }
        }

        public class CapNhatThongTinNguoiBenhDto : GetMenuDto
        {
            public string HoVaTen { get; set; }

            public int NgaySinh { get; set; }

            public string GioiTinh { get; set; }

            public string DiaChi { get; set; }

            public string PhoneNumber { get; set; }

            public string EmailAddress { get; set; }

            public string ProfilePicture { get; set; }

            public int ThangSinh { get; set; }

            public int NamSinh { get; set; }

            public string SoTheBHYT { get; set; }

            public string NoiDkBanDau { get; set; }

            public string MaDonViBHXH { get; set; }

            public DateTime GiaTriSuDungTuNgay { get; set; }

            public DateTime ThoiDiemDuNam { get; set; }
        }

        public class ThongTinNguoiBenhResultDto
        {
            public string HoVaTen { get; set; }

            public int NgaySinh { get; set; }

            public string GioiTinh { get; set; }

            public string DiaChi { get; set; }

            public string PhoneNumber { get; set; }

            public string EmailAddress { get; set; }

            public string ProfilePicture { get; set; }

            public int ThangSinh { get; set; }

            public int NamSinh { get; set; }

            public string SoTheBHYT { get; set; }

            public string NoiDkBanDau { get; set; }

            public string MaDonViBHXH { get; set; }

            public DateTime GiaTriSuDungTuNgay { get; set; }

            public DateTime ThoiDiemDuNam { get; set; }
        }

        public class KetQuaTraveThongTinNguoiBenhResultDto : GetTenantsResultDto
        {
            public ThongTinNguoiBenhResultDto ThongTinNguoiBenhResultDto { get; set; }
        }

        public class DanhSachBacSiChuyenKhoaTenantDto
        {
            public long BacSiId { get; set; }
            public int ChuyenKhoaId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string Image { get; set; }
            public string TieuSu { get; set; }
            public string ChucDanh { get; set; }
            public int SoLanDat { get; set; }
        }

        public class GetDanhSachBacSiChuyenKhoaTenantResultDto : GetTenantsResultDto
        {
            public List<DanhSachBacSiChuyenKhoaTenantDto> ListDanhSachBacSiChuyenKhoaTenantDto { get; set; }
        }

        public class GetDanhSachBacSiChuyenKhoaTenantDto : TenantDto
        {
            public int ChuyenKhoaId { get; set; }
            public int SoBacSiCanLayTrongDs { get; set; }
        }

        public class CapNhatThongTinDeviceToken
        {
            public string Token { get; set; }
            public string DeviceToken { get; set; }
            public string UserName { get; set; }
        }

        public class GetThongTinNguoiBenh
        {
            public string Token { get; set; }
        }

        public class GetThongBaoNguoiBenhResult
        {
            public long IdThongBao { get; set; }
            public int NguoiBenhId { get; set; }
            public DateTime ThoiGianGui { get; set; }
            public string TieuDe { get; set; }
            public string NoiDungTinNhan { get; set; }
            public int TrangThai { get; set; }
        }

        public class GetThongBaoNguoiBenhResultDto : CommonResponse
        {
            public GetThongBaoNguoiBenhResultDto()
            {
                ListThongBaoNguoiBenh = new List<GetThongBaoNguoiBenhResult>();
            }
            public List<GetThongBaoNguoiBenhResult> ListThongBaoNguoiBenh { get; set; }
        }

        public class SetTrangThaiThongBaoNguoiBenhDto : GetMenuDto
        {
            public long IdThongBao { get; set; }
        }

        public class GioKhamTheoKhung
        {
            public int KhungKham { get; set; }
            public string GioKham { get; set; }
            public bool IsCoTheDat { get; set; }
            public string MoTa { get; set; }
            
        }
        public class GetGioKhamTheoKhungResultDto : CommonResponse
        {
            public List<LichHenKhamDto> LichHenKham { get; set; }
            public int KhamSession { get; set; }
            public string GioBatDauLamViecSang { get; set; }
            public string GioKetThucLamViecSang { get; set; }
            public string GioBatDauLamViecChieu { get; set; }
            public string GioKetThucLamViecChieu { get; set; }
            public List<GioKhamTheoKhung> GioKhamTheoKhungSang { get; set; }
            public List<GioKhamTheoKhung> GioKhamTheoKhungChieu { get; set; }
        }

        public class GetGioKhamTheoKhungDto : TenantDto
        {
            public DateTime NgayHenKham { get; set; }
            public int ChuyenKhoaId { get; set; }
        }

        public class EditLichHenKhamDto : TenantDto
        {
            public DateTime NgayHenKham { get; set; }

            public string MoTaTrieuChung { get; set; }

            public bool IsCoBHYT { get; set; }

            public string SoTheBHYT { get; set; }

            public string NoiDangKyKCBDauTien { get; set; }

            public DateTime? BHYTValidDate { get; set; }

            public int PhuongThucThanhToan { get; set; }

            public long? BacSiId { get; set; }

            public int? NguoiThanId { get; set; }

            public int? ChuyenKhoaId { get; set; }

            public int? KhungKham { get; set; }

            public int LichHenKhamId { get; set; }
        }

        public class HuyLichHenKhamDto : TenantDto
        {
            public int LichHenKhamId { get; set; }
        }
        
        public class GetImageBacSi
        {
            public string Image { get; set; }

        }
        
        public class GetImageBacSiOutput
        {
            public string ProfilePicture { get; set; }

            public GetImageBacSiOutput(string profilePicture)
            {
                ProfilePicture = profilePicture;
            }
        }

        public class NguoiBenhDto : GetMenuDto
        {
            public string HoVaTen { get; set; }

            public int NgaySinh { get; set; }

            public string GioiTinh { get; set; }

            public string DiaChi { get; set; }

            public string PhoneNumber { get; set; }

            public string EmailAddress { get; set; }

            public string ProfilePicture { get; set; }

            public int ThangSinh { get; set; }

            public int NamSinh { get; set; }

            public string SoTheBHYT { get; set; }

            public string NoiDkBanDau { get; set; }

            public string MaDonViBHXH { get; set; }

            public DateTime GiaTriSuDungTuNgay { get; set; }

            public DateTime ThoiDiemDuNam { get; set; }
        }

        public class GetDanhSachNhanVienResultDto : CommonResponse
        {
            public List<NguoiBenhDto> DanhSachNhanVien { get; set; }
        }
        public class DiemDanhInputDto : GetMenuDto
        {
            public string CapturedImageBase64 { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string DeviceInfo { get; set; }
        }
        public class GetLichSuDiemDanhDto : GetMenuDto
        {
            public DateTime? tuNgay { get; set; }
            public DateTime? denNgay { get; set; }
        }
        public class NgayThongKeDto
        {
            public DateTime Ngay { get; set; }
            public string Loai { get; set; }
            public DateTime FirstCheck { get; set; }
            public DateTime LastCheck { get; set; }
            public int LateMinutes { get; set; }
            public int EarlyMinutes { get; set; }
            public double OvertimeHours { get; set; }
            public double WorkedHours { get; set; }
            public string ShiftName { get; set; }
            public string WorkMode { get; set; }
        }
        public class LichSuDiemDanhDto
        {
            public DateTime CheckIn { get; set; }
            public string CheckInDeviceInfo { get; set; }
            public double CheckInLatitude { get; set; }
            public double CheckInLongitude { get; set; }
            
            public int? ShiftId { get; set; }
            public string ShiftName { get; set; }
            
            public string WorkMode { get; set; }
            public bool IsLateCheckIn { get; set; }
        }
        public class DiMuon
        {
            public int SoNgayDiMuon { get; set; }
            public double TongGioDiMuon { get; set; }
        }
        public class VeSom
        {
            public int SoNgayVeSom { get; set; }
            public double TongGioVeSom { get; set; }
        }
        public class NgayLoaiDto
        {
            public DateTime Ngay { get; set; }
            public string Loai { get; set; }
        }
        public class GetLichSuDiemDanhResultDto : CommonResponse
        {
            public bool status { get; set; }
            public string message { get; set; }
            public int errorCode { get; set; }
            public List<LichSuDiemDanhDto> LichSuDiemDanh { get; set; }
            public DiMuon DiMuon { get; set; }
            public VeSom VeSom { get; set; }
            public int SoNgayNghi { get; set; }
            public List<DateTime> DanhSachNgayNghi { get; set; }
            public int SoNgayLamTheoCa { get; set; }
            public int SoNgayLamHanhChinh { get; set; }
            public List<NgayLoaiDto> DanhSachNgayLoai { get; set; }
            public List<NgayThongKeDto> DanhSachNgayThongKe { get; set; }
        }
        public class ThongKeDiemDanhInputDto : GetMenuDto
        {
            public LOAI_BAO_CAO loaiBaoCao { get; set; }
            public DateTime? tuNgay { get; set; }
            public DateTime? denNgay { get; set; }
        }
        public class UpdateDeviceTokenDto
        {
            public int userid { get; set; }
            public string devicetoken { get; set; }
            public string token { get; set; }
            public string username { get; set; }
        }
        
        public class SendNotificationDto
        {
            public string DeviceToken { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
        public class ThongKeDiemDanhResultDto : CommonResponse
        {
            public TongHopDiemDanhDto TongHop { get; set; }
            public List<ThongKeDiemDanhTheoKyDto> DanhSach { get; set; }
        }

        public class ThongKeDiemDanhTheoKyDto
        {
            public string Ky { get; set; }
            public int SoLanCheckIn { get; set; }
            public int SoNgayDiMuon { get; set; }
            public double TongGioDiMuon { get; set; }
            public int SoNgayVeSom { get; set; }
            public double TongGioVeSom { get; set; }
        }
        public class TongHopDiemDanhDto
        {
            public int TongNgayDiMuon { get; set; }
            public int TongNgayVeSom { get; set; }
            public int TongNgayDungGio { get; set; }
        }
    }

    public class PortalDto
    {
        public class PortalGetListChuyenKhoaInput
        {
            public string token { get; set; }
        }

        public class PortalGetListChuyenKhoaResult
        {
            public List<ChuyenKhoaDto> ChuyenKhoaDtos { get; set; }
        }

        public class PortalRegisterInput : PortalGetListChuyenKhoaInput
        {
            public string hovaten { get; set; }
            public DateTime ngaygiohenkham { get; set; }
            public string motatrieuchung { get; set; }
            public string cccd { get; set; }
            public int chuyenkhoa { get; set; }
            public string sdt { get; set; }
        }
    }
    
}