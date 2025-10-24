using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.NguoiBenhTestNS.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}