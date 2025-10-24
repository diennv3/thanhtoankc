using Abp.AutoMapper;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.MultiTenancy.Dto;
using Karion.BusinessSolution.Web.Areas.App.Models.Common;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}