
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditDichVuDto : EntityDto<int?>
    {

		[Required]
		[StringLength(DichVuConsts.MaxTenLength, MinimumLength = DichVuConsts.MinTenLength)]
		public string Ten { get; set; }
		
		
		public string MoTa { get; set; }
		
		
		 public int? ChuyenKhoaId { get; set; }
		 
		 
    }
}