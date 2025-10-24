
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditNguoiThanDto : EntityDto<int?>
    {

		[Required]
		[StringLength(NguoiThanConsts.MaxHoVaTenLength, MinimumLength = NguoiThanConsts.MinHoVaTenLength)]
		public string HoVaTen { get; set; }
		
		
		public int Tuoi { get; set; }
		
		
		public string GioiTinh { get; set; }
		
		
		public string DiaChi { get; set; }
		
		
		[StringLength(NguoiThanConsts.MaxMoiQuanHeLength, MinimumLength = NguoiThanConsts.MinMoiQuanHeLength)]
		public string MoiQuanHe { get; set; }
		
		
		[StringLength(NguoiThanConsts.MaxSoDienThoaiLength, MinimumLength = NguoiThanConsts.MinSoDienThoaiLength)]
		public string SoDienThoai { get; set; }
		
		
		 public int? NguoiBenhId { get; set; }
		 
		 
    }
}