using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Controllers;

namespace Karion.BusinessSolution.Web.Public.Controllers
{
    public class AboutController : BusinessSolutionControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}