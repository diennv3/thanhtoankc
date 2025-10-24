using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllNguoiThansForExcelInput
    {
		public string Filter { get; set; }

		public string HoVaTenFilter { get; set; }

		public int? MaxTuoiFilter { get; set; }
		public int? MinTuoiFilter { get; set; }

		public string GioiTinhFilter { get; set; }

		public string DiaChiFilter { get; set; }

		public string MoiQuanHeFilter { get; set; }

		public string SoDienThoaiFilter { get; set; }


		 public string NguoiBenhHoVaTenFilter { get; set; }

		 
    }
}