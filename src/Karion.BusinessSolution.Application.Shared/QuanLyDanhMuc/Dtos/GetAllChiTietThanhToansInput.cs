using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllChiTietThanhToansInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public decimal? MaxSoTienThanhToanFilter { get; set; }
		public decimal? MinSoTienThanhToanFilter { get; set; }

		public int? MaxLoaiThanhToanFilter { get; set; }
		public int? MinLoaiThanhToanFilter { get; set; }

		public DateTime? MaxNgayThanhToanFilter { get; set; }
		public DateTime? MinNgayThanhToanFilter { get; set; }


		 public string LichHenKhamMoTaTrieuChungFilter { get; set; }

		 		 public string NguoiBenhUserNameFilter { get; set; }

		 
    }
}