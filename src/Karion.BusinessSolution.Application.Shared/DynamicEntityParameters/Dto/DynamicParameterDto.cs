using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.DynamicEntityParameters.Dto
{
    public class DynamicParameterDto : EntityDto
    {
        public string ParameterName { get; set; }

        public string InputType { get; set; }

        public string Permission { get; set; }

        public int? TenantId { get; set; }
    }
}
