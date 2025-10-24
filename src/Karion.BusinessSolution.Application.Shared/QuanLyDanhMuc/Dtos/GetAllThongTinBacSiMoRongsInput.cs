using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllThongTinBacSiMoRongsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ImageFilter { get; set; }

		public string TieuSuFilter { get; set; }

		public string ChucDanhFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}