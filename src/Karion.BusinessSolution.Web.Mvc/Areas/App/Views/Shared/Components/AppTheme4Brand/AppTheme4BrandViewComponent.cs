using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.Layout;
using Karion.BusinessSolution.Web.Session;
using Karion.BusinessSolution.Web.Views;

namespace Karion.BusinessSolution.Web.Areas.App.Views.Shared.Components.AppTheme4Brand
{
    public class AppTheme4BrandViewComponent : BusinessSolutionViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme4BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
