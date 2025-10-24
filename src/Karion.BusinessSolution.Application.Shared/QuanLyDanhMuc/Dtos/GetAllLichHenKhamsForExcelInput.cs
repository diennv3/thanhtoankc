using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllLichHenKhamsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxNgayHenKhamFilter { get; set; }
		public DateTime? MinNgayHenKhamFilter { get; set; }

		public string MoTaTrieuChungFilter { get; set; }

		public int IsCoBHYTFilter { get; set; }

		public string SoTheBHYTFilter { get; set; }

		public string NoiDangKyKCBDauTienFilter { get; set; }

		public DateTime? MaxBHYTValidDateFilter { get; set; }
		public DateTime? MinBHYTValidDateFilter { get; set; }

		public int? MaxPhuongThucThanhToanFilter { get; set; }
		public int? MinPhuongThucThanhToanFilter { get; set; }

		public int IsDaKhamFilter { get; set; }

		public int IsDaThanhToanFilter { get; set; }

		public DateTime? MaxTimeHoanThanhKhamFilter { get; set; }
		public DateTime? MinTimeHoanThanhKhamFilter { get; set; }

		public DateTime? MaxTimeHoanThanhThanhToanFilter { get; set; }
		public DateTime? MinTimeHoanThanhThanhToanFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string UserName2Filter { get; set; }

		 		 public string NguoiBenhUserNameFilter { get; set; }

		 		 public string NguoiThanHoVaTenFilter { get; set; }

		 		 public string DichVuTenFilter { get; set; }

		 		 public string ChuyenKhoaTenFilter { get; set; }

		 
    }
}