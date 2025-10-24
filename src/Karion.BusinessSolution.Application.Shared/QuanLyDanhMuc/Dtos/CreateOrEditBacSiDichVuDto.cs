
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditBacSiDichVuDto : EntityDto<int?>
    {

		 public long UserId { get; set; }
		 
		 		 public int DichVuId { get; set; }
		 
		 
    }
}