
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditNguoiBenhNotificationDto : EntityDto<long?>
    {

		public string NoiDungTinNhan { get; set; }
		
		
		public int TrangThai { get; set; }
		
		
		public string TieuDe { get; set; }
		
		
		public DateTime ThoiGianGui { get; set; }
		
		
		 public int NguoiBenhId { get; set; }
		 
		 
    }
}