using System.Collections.Generic;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

namespace Karion.BusinessSolution.Web.Areas.App.Models.LichHenKhams
{
    public class LichHenKhamViewModel : GetLichHenKhamForViewDto
    {
        public List<LichHenKhamForViewDto> Detail { get; set; }
        public List<ChiTietThanhToanDto> ChiTietThanhToan { get; set; }
    }
}