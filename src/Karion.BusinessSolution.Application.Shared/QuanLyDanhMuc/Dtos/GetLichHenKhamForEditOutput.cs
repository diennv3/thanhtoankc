using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetLichHenKhamForEditOutput
    {
		public CreateOrEditLichHenKhamDto LichHenKham { get; set; }

		public string UserName { get; set;}

		public string UserName2 { get; set;}

		public string NguoiBenhUserName { get; set;}

		public string NguoiThanHoVaTen { get; set;}

		public string ChuyenKhoaTen { get; set;}


    }
}