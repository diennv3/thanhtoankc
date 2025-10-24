using Abp.AspNetCore.Mvc.ViewComponents;

namespace Karion.BusinessSolution.Web.Public.Views
{
    public abstract class BusinessSolutionViewComponent : AbpViewComponent
    {
        protected BusinessSolutionViewComponent()
        {
            LocalizationSourceName = BusinessSolutionConsts.LocalizationSourceName;
        }
    }
}