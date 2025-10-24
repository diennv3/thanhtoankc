using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetNguoiThanForEditOutput
    {
		public CreateOrEditNguoiThanDto NguoiThan { get; set; }

		public string NguoiBenhHoVaTen { get; set;}


    }
}