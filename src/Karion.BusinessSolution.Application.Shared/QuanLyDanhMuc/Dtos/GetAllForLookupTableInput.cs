using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}