using Abp.AutoMapper;
using Karion.BusinessSolution.MultiTenancy.Dto;

namespace Karion.BusinessSolution.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
