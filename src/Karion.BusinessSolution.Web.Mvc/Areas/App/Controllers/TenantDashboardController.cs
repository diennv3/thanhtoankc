using System.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.DashboardCustomization;
using Karion.BusinessSolution.Web.DashboardCustomization;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDanhMuc;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration,
            IDashboardCustomizationAppService dashboardCustomizationAppService 
        )
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {
        }

        public async Task<ActionResult> Index()
        {
            return await GetView(BusinessSolutionDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}