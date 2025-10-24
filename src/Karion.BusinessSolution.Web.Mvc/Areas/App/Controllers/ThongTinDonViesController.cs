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
    [AbpMvcAuthorize(AppPermissions.Pages_ThongTinDonVies)]
    public class ThongTinDonViesController : BusinessSolutionControllerBase
    {
        private readonly IThongTinDonViesAppService _thongTinDonViesAppService;

        public ThongTinDonViesController(IThongTinDonViesAppService thongTinDonViesAppService)
        {
            _thongTinDonViesAppService = thongTinDonViesAppService;
        }

        public ActionResult Index()
        {
            var model = new ThongTinDonViesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ThongTinDonVies_Create, AppPermissions.Pages_ThongTinDonVies_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetThongTinDonViForEditOutput getThongTinDonViForEditOutput;

				if (id.HasValue){
					getThongTinDonViForEditOutput = await _thongTinDonViesAppService.GetThongTinDonViForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getThongTinDonViForEditOutput = new GetThongTinDonViForEditOutput{
						ThongTinDonVi = new CreateOrEditThongTinDonViDto()
					};
				}

				var viewModel = new CreateOrEditThongTinDonViModalViewModel()
				{
					ThongTinDonVi = getThongTinDonViForEditOutput.ThongTinDonVi,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewThongTinDonViModal(int id)
        {
			var getThongTinDonViForViewDto = await _thongTinDonViesAppService.GetThongTinDonViForView(id);

            var model = new ThongTinDonViViewModel()
            {
                ThongTinDonVi = getThongTinDonViForViewDto.ThongTinDonVi
            };

            return PartialView("_ViewThongTinDonViModal", model);
        }


    }
}