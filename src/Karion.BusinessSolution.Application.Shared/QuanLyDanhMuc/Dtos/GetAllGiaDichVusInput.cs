using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllGiaDichVusInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string MucGiaFilter { get; set; }

		public string MoTaFilter { get; set; }

		public int? MaxGiaFilter { get; set; }
		public int? MinGiaFilter { get; set; }

		public DateTime? MaxNgayApDungFilter { get; set; }
		public DateTime? MinNgayApDungFilter { get; set; }


		 public string DichVuTenFilter { get; set; }

		 
    }
}