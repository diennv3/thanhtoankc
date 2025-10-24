using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}