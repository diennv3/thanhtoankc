using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetChiTietThanhToanForEditOutput
    {
		public CreateOrEditChiTietThanhToanDto ChiTietThanhToan { get; set; }

		public string LichHenKhamMoTaTrieuChung { get; set;}

		public string NguoiBenhUserName { get; set;}


    }
}