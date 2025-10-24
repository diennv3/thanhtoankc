
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditThongTinBacSiMoRongDto : EntityDto<int?>
    {

		public string Image { get; set; }
		
		
		public string TieuSu { get; set; }
		
		
		public string ChucDanh { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}