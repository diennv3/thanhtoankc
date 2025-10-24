using Abp.AutoMapper;
using Karion.BusinessSolution.Sessions.Dto;

namespace Karion.BusinessSolution.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}