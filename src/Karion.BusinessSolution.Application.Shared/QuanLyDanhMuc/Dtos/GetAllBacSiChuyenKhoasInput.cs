using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllBacSiChuyenKhoasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string ChuyenKhoaTenFilter { get; set; }

		 
    }
}