using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllDichVusInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TenFilter { get; set; }

		public string MoTaFilter { get; set; }


		 public string ChuyenKhoaTenFilter { get; set; }

		 
    }
}