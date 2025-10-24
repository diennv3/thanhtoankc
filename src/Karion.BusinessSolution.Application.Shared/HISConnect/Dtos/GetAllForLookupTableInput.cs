using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.HISConnect.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}