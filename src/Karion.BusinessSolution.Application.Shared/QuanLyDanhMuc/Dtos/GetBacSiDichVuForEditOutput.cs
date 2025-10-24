using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetBacSiDichVuForEditOutput
    {
		public CreateOrEditBacSiDichVuDto BacSiDichVu { get; set; }

		public string UserName { get; set;}

		public string DichVuTen { get; set;}


    }
}