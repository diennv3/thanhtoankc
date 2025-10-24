using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetBacSiChuyenKhoaForEditOutput
    {
		public CreateOrEditBacSiChuyenKhoaDto BacSiChuyenKhoa { get; set; }

		public string UserName { get; set;}

		public string ChuyenKhoaTen { get; set;}


    }
}