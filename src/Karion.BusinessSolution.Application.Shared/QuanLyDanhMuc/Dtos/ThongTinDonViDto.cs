
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class ThongTinDonViDto : EntityDto
    {
		public string Key { get; set; }

		public string Value { get; set; }
		public int Id { get; set; }
    }
}