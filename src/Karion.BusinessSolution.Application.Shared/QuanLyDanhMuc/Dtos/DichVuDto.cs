
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class DichVuDto : EntityDto
    {
		public string Ten { get; set; }

		public string MoTa { get; set; }


		 public int? ChuyenKhoaId { get; set; }

		 
    }
}