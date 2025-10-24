
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditChiTietThanhToanDto : EntityDto<int?>
    {

		public decimal SoTienThanhToan { get; set; }
		
		
		public int LoaiThanhToan { get; set; }
		
		
		public DateTime NgayThanhToan { get; set; }
		
		
		 public int? LichHenKhamId { get; set; }
		 
		 		 public int? NguoiBenhId { get; set; }
		 
		 
    }
}