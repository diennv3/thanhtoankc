
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class NguoiBenhNotificationDto : EntityDto<long>
    {
		public string NoiDungTinNhan { get; set; }

		public int TrangThai { get; set; }

		public string TieuDe { get; set; }

		public DateTime ThoiGianGui { get; set; }


		 public int NguoiBenhId { get; set; }

		 
    }
}