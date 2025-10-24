using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.HanetFaceDetecteds;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HanetFaceDetecteds)]
    public class HanetFaceDetectedsController : BusinessSolutionControllerBase
    {
        private readonly IHanetFaceDetectedsAppService _hanetFaceDetectedsAppService;

        public HanetFaceDetectedsController(IHanetFaceDetectedsAppService hanetFaceDetectedsAppService)
        {
            _hanetFaceDetectedsAppService = hanetFaceDetectedsAppService;
        }

        public ActionResult Index()
        {
            var model = new HanetFaceDetectedsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_HanetFaceDetecteds_Create, AppPermissions.Pages_HanetFaceDetecteds_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetHanetFaceDetectedForEditOutput getHanetFaceDetectedForEditOutput;

				if (id.HasValue){
					getHanetFaceDetectedForEditOutput = await _hanetFaceDetectedsAppService.GetHanetFaceDetectedForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getHanetFaceDetectedForEditOutput = new GetHanetFaceDetectedForEditOutput{
						HanetFaceDetected = new CreateOrEditHanetFaceDetectedDto()
					};
				}

				var viewModel = new CreateOrEditHanetFaceDetectedModalViewModel()
				{
					HanetFaceDetected = getHanetFaceDetectedForEditOutput.HanetFaceDetected,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewHanetFaceDetectedModal(long id)
        {
			var getHanetFaceDetectedForViewDto = await _hanetFaceDetectedsAppService.GetHanetFaceDetectedForView(id);

            var model = new HanetFaceDetectedViewModel()
            {
                HanetFaceDetected = getHanetFaceDetectedForViewDto.HanetFaceDetected
            };

            return PartialView("_ViewHanetFaceDetectedModal", model);
        }


    }
}