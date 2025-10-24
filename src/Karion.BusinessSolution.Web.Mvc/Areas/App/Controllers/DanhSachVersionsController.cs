using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.DanhSachVersions;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.VersionControl;
using Karion.BusinessSolution.VersionControl.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DanhSachVersions)]
    public class DanhSachVersionsController : BusinessSolutionControllerBase
    {
        private readonly IDanhSachVersionsAppService _danhSachVersionsAppService;

        public DanhSachVersionsController(IDanhSachVersionsAppService danhSachVersionsAppService)
        {
            _danhSachVersionsAppService = danhSachVersionsAppService;
        }

        public ActionResult Index()
        {
            var model = new DanhSachVersionsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_DanhSachVersions_Create, AppPermissions.Pages_DanhSachVersions_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetDanhSachVersionForEditOutput getDanhSachVersionForEditOutput;

				if (id.HasValue){
					getDanhSachVersionForEditOutput = await _danhSachVersionsAppService.GetDanhSachVersionForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getDanhSachVersionForEditOutput = new GetDanhSachVersionForEditOutput{
						DanhSachVersion = new CreateOrEditDanhSachVersionDto()
					};
				}

				var viewModel = new CreateOrEditDanhSachVersionModalViewModel()
				{
					DanhSachVersion = getDanhSachVersionForEditOutput.DanhSachVersion,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewDanhSachVersionModal(int id)
        {
			var getDanhSachVersionForViewDto = await _danhSachVersionsAppService.GetDanhSachVersionForView(id);

            var model = new DanhSachVersionViewModel()
            {
                DanhSachVersion = getDanhSachVersionForViewDto.DanhSachVersion
            };

            return PartialView("_ViewDanhSachVersionModal", model);
        }


    }
}