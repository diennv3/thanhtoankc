using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Editions.Dto;

namespace Karion.BusinessSolution.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}