using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Karion.BusinessSolution.Web.Controllers
{
    public class HomeController : BusinessSolutionControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
