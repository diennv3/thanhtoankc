using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class LichHenKhamDto : EntityDto
    {
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

        public decimal TongTienThanhToan { get; set; }
        public decimal TienThua { get; set; }

        public long? ThuNganId { get; set; }

        public int? NguoiBenhId { get; set; }

        public int? NguoiThanId { get; set; }

        public int? ChuyenKhoaId { get; set; }
        public int? KhungKham { get; set; }
        
        public  decimal TongChiPhi { get; set; }
        
        public  bool  IsTamUng { get; set; }

    }
}