using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditLichHenKhamDto : EntityDto<int?>
    {
        public DateTime NgayHenKham { get; set; }


        [Required] public string MoTaTrieuChung { get; set; }


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


        public long? BacSiId { get; set; }

        public long? ThuNganId { get; set; }

        public int? NguoiBenhId { get; set; }

        public int? NguoiThanId { get; set; }

        public int? ChuyenKhoaId { get; set; }
        public int? KhungKham { get; set; }
    }
}