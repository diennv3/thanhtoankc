using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
