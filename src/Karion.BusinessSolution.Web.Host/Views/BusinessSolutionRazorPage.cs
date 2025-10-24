using Abp.AspNetCore.Mvc.Views;

namespace Karion.BusinessSolution.Web.Views
{
    public abstract class BusinessSolutionRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected BusinessSolutionRazorPage()
        {
            LocalizationSourceName = BusinessSolutionConsts.LocalizationSourceName;
        }
    }
}
