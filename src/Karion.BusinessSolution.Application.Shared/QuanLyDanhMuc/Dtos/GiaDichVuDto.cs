
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GiaDichVuDto : EntityDto
    {
		public string MucGia { get; set; }

		public string MoTa { get; set; }

		public int Gia { get; set; }

		public DateTime NgayApDung { get; set; }


		 public int? DichVuId { get; set; }

		 
    }
}