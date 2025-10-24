using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}