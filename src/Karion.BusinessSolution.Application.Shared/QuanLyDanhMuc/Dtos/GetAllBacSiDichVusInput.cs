using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllBacSiDichVusInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string DichVuTenFilter { get; set; }

		 
    }
}