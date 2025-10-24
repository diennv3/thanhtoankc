using Microsoft.AspNetCore.Antiforgery;

namespace Karion.BusinessSolution.Web.Controllers
{
    public class AntiForgeryController : BusinessSolutionControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
