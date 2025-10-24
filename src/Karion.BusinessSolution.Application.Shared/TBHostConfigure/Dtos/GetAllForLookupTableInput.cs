using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.TBHostConfigure.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}