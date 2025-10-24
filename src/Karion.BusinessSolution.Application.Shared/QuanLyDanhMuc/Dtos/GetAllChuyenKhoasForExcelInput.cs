using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllChuyenKhoasForExcelInput
    {
		public string Filter { get; set; }

		public string TenFilter { get; set; }

		public string MoTaFilter { get; set; }



    }
}