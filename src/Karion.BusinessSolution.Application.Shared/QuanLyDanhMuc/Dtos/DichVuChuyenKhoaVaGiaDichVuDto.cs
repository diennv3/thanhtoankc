using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class DichVuChuyenKhoaVaGiaDichVuDto
    {
        public int Id { get; set; }
        public string MoTa { get; set; }
        public string TenDichVu { get; set; }
        public int? Gia { get; set; }
    }
}
