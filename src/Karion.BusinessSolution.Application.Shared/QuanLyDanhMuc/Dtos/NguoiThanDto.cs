
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class NguoiThanDto : EntityDto
    {
		public string HoVaTen { get; set; }

		public int Tuoi { get; set; }

		public string GioiTinh { get; set; }

		public string DiaChi { get; set; }

		public string MoiQuanHe { get; set; }

		public string SoDienThoai { get; set; }


		 public int? NguoiBenhId { get; set; }

		 
    }
}