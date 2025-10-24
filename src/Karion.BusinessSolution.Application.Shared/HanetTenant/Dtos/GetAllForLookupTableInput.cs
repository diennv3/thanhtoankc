using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}