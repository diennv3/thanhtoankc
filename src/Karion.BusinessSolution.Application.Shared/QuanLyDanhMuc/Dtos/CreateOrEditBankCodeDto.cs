
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditBankCodeDto : EntityDto<int?>
    {

		[Required]
		public string Code { get; set; }
		
		
		[Required]
		public string BankName { get; set; }
		
		

    }
}