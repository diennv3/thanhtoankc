using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.ThongTinDonVies;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class ThongKeBaoCaosController : BusinessSolutionControllerBase
    {
        private readonly IThongKeBaoCaosAppService _thongKeBaoCaosAppService;

        public ThongKeBaoCaosController(IThongKeBaoCaosAppService thongKeBaoCaosAppService)
        {
            _thongKeBaoCaosAppService = thongKeBaoCaosAppService;
        }

        public ActionResult Index()
        {
            return View();
        } 

    }
}