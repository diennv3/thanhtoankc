using Abp.AutoMapper;
using Karion.BusinessSolution.MultiTenancy.Dto;

namespace Karion.BusinessSolution.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}