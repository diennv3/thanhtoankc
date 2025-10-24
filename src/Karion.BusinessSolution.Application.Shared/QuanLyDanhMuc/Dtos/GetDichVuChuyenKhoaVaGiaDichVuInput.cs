using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetDichVuChuyenKhoaVaGiaDichVuInput : PagedAndSortedResultRequestDto
    {
        public int ChuyenKhoaId { get; set; }
    }
}
