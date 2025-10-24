
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditChuyenKhoaDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ChuyenKhoaConsts.MaxTenLength, MinimumLength = ChuyenKhoaConsts.MinTenLength)]
		public string Ten { get; set; }
		
		
		public string MoTa { get; set; }
		
		

    }
}