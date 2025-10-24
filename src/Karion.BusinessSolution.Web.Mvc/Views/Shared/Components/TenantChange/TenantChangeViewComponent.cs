using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Session;

namespace Karion.BusinessSolution.Web.Views.Shared.Components.TenantChange
{
    public class TenantChangeViewComponent : BusinessSolutionViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public TenantChangeViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            var model = ObjectMapper.Map<TenantChangeViewModel>(loginInfo);
            return View(model);
        }
    }
}
