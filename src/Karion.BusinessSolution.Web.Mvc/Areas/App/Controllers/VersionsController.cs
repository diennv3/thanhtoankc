using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.Versions;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.VersionControl;
using Karion.BusinessSolution.VersionControl.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Versions)]
    public class VersionsController : BusinessSolutionControllerBase
    {
        private readonly IVersionsAppService _versionsAppService;

        public VersionsController(IVersionsAppService versionsAppService)
        {
            _versionsAppService = versionsAppService;
        }

        public ActionResult Index()
        {
            var model = new VersionsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Versions_Create, AppPermissions.Pages_Versions_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetVersionForEditOutput getVersionForEditOutput;

				if (id.HasValue){
					getVersionForEditOutput = await _versionsAppService.GetVersionForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getVersionForEditOutput = new GetVersionForEditOutput{
						Version = new CreateOrEditVersionDto()
					};
				}

				var viewModel = new CreateOrEditVersionModalViewModel()
				{
					Version = getVersionForEditOutput.Version,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewVersionModal(int id)
        {
			var getVersionForViewDto = await _versionsAppService.GetVersionForView(id);

            var model = new VersionViewModel()
            {
                Version = getVersionForViewDto.Version
            };

            return PartialView("_ViewVersionModal", model);
        }


    }
}