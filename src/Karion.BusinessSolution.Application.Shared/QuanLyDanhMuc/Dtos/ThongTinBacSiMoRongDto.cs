
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class ThongTinBacSiMoRongDto : EntityDto
    {
		public string Image { get; set; }
		public string TieuSu { get; set; }

		public string ChucDanh { get; set; }
		public long? UserId { get; set; }
		public string ImageThumbnail { get; set; }
		 
    }
}