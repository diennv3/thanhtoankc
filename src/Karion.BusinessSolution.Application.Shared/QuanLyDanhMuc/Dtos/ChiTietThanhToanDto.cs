
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class ChiTietThanhToanDto : EntityDto
    {
		public decimal SoTienThanhToan { get; set; }

		public int LoaiThanhToan { get; set; }

		public DateTime NgayThanhToan { get; set; }


		 public int? LichHenKhamId { get; set; }

		 public int? NguoiBenhId { get; set; }
		 public bool TrangThaiThanhToan { get; set; }

		 public string QRString { get; set; }
    }
}