
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditThongTinDonViDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ThongTinDonViConsts.MaxKeyLength, MinimumLength = ThongTinDonViConsts.MinKeyLength)]
		public string Key { get; set; }
		
		
		public string Value { get; set; }
		
		

    }
}