using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.Layout;
using Karion.BusinessSolution.Web.Session;
using Karion.BusinessSolution.Web.Views;

namespace Karion.BusinessSolution.Web.Areas.App.Views.Shared.Components.AppDefaultFooter
{
    public class AppDefaultFooterViewComponent : BusinessSolutionViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppDefaultFooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
