using Abp.AspNetCore.Mvc.Authorization;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TiepNhanBenhNhan)]
    public class TiepNhanBenhNhanController : BusinessSolutionControllerBase
    {
        public TiepNhanBenhNhanController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
