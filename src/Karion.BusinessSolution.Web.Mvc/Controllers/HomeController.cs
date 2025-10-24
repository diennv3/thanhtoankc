using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Identity;

namespace Karion.BusinessSolution.Web.Controllers
{
    public class HomeController : BusinessSolutionControllerBase
    {
        private readonly SignInManager _signInManager;

        public HomeController(SignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string redirect = "", bool forceNewRegistration = false)
        {
            if (forceNewRegistration)
            {
                await _signInManager.SignOutAsync();
            }

            if (redirect == "TenantRegistration")
            {
                return RedirectToAction("SelectEdition", "TenantRegistration");
            }

            return AbpSession.UserId.HasValue ?
                RedirectToAction("Index", "Home", new { area = "App" }) :
                RedirectToAction("Login", "Account");
        }
    }
}