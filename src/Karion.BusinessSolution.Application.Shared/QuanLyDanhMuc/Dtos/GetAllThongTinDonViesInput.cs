using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllThongTinDonViesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string KeyFilter { get; set; }

		public string ValueFilter { get; set; }



    }
}