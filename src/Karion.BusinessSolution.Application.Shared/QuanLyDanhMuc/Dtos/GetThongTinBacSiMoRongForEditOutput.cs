using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetThongTinBacSiMoRongForEditOutput
    {
		public CreateOrEditThongTinBacSiMoRongDto ThongTinBacSiMoRong { get; set; }

		public string UserName { get; set;}


    }
}