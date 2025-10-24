
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditGiaDichVuDto : EntityDto<int?>
    {

		[Required]
		[StringLength(GiaDichVuConsts.MaxMucGiaLength, MinimumLength = GiaDichVuConsts.MinMucGiaLength)]
		public string MucGia { get; set; }
		
		
		public string MoTa { get; set; }
		
		
		public int Gia { get; set; }
		
		
		public DateTime NgayApDung { get; set; }
		
		
		 public int? DichVuId { get; set; }
		 
		 
    }
}