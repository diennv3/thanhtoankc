using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.HanetTenantDeviceDatases;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases)]
    public class HanetTenantDeviceDatasesController : BusinessSolutionControllerBase
    {
        private readonly IHanetTenantDeviceDatasesAppService _hanetTenantDeviceDatasesAppService;

        public HanetTenantDeviceDatasesController(IHanetTenantDeviceDatasesAppService hanetTenantDeviceDatasesAppService)
        {
            _hanetTenantDeviceDatasesAppService = hanetTenantDeviceDatasesAppService;
        }

        public ActionResult Index()
        {
            var model = new HanetTenantDeviceDatasesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases_Create, AppPermissions.Pages_HanetTenantDeviceDatases_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetHanetTenantDeviceDatasForEditOutput getHanetTenantDeviceDatasForEditOutput;

				if (id.HasValue){
					getHanetTenantDeviceDatasForEditOutput = await _hanetTenantDeviceDatasesAppService.GetHanetTenantDeviceDatasForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getHanetTenantDeviceDatasForEditOutput = new GetHanetTenantDeviceDatasForEditOutput{
						HanetTenantDeviceDatas = new CreateOrEditHanetTenantDeviceDatasDto()
					};
				getHanetTenantDeviceDatasForEditOutput.HanetTenantDeviceDatas.lastCheck = DateTime.Now;
				}

				var viewModel = new CreateOrEditHanetTenantDeviceDatasModalViewModel()
				{
					HanetTenantDeviceDatas = getHanetTenantDeviceDatasForEditOutput.HanetTenantDeviceDatas,
					HanetTenantPlaceDatasplaceName = getHanetTenantDeviceDatasForEditOutput.HanetTenantPlaceDatasplaceName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewHanetTenantDeviceDatasModal(int id)
        {
			var getHanetTenantDeviceDatasForViewDto = await _hanetTenantDeviceDatasesAppService.GetHanetTenantDeviceDatasForView(id);

            var model = new HanetTenantDeviceDatasViewModel()
            {
                HanetTenantDeviceDatas = getHanetTenantDeviceDatasForViewDto.HanetTenantDeviceDatas
                , HanetTenantPlaceDatasplaceName = getHanetTenantDeviceDatasForViewDto.HanetTenantPlaceDatasplaceName 

            };

            return PartialView("_ViewHanetTenantDeviceDatasModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases_Create, AppPermissions.Pages_HanetTenantDeviceDatases_Edit)]
        public PartialViewResult HanetTenantPlaceDatasLookupTableModal(int? id, string displayName)
        {
            var viewModel = new HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableModal", viewModel);
        }

    }
}