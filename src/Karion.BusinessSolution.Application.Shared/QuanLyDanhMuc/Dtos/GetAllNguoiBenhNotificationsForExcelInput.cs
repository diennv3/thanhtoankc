using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllNguoiBenhNotificationsForExcelInput
    {
		public string Filter { get; set; }

		public string NoiDungTinNhanFilter { get; set; }

		public int? MaxTrangThaiFilter { get; set; }
		public int? MinTrangThaiFilter { get; set; }

		public string TieuDeFilter { get; set; }

		public DateTime? MaxThoiGianGuiFilter { get; set; }
		public DateTime? MinThoiGianGuiFilter { get; set; }


		 public string NguoiBenhUserNameFilter { get; set; }

		 
    }
}