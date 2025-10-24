using Abp.AutoMapper;
using Karion.BusinessSolution.Organizations.Dto;

namespace Karion.BusinessSolution.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}