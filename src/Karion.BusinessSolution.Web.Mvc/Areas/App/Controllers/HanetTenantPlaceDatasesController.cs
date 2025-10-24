using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.HanetTenantPlaceDatases;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases)]
    public class HanetTenantPlaceDatasesController : BusinessSolutionControllerBase
    {
        private readonly IHanetTenantPlaceDatasesAppService _hanetTenantPlaceDatasesAppService;

        public HanetTenantPlaceDatasesController(IHanetTenantPlaceDatasesAppService hanetTenantPlaceDatasesAppService)
        {
            _hanetTenantPlaceDatasesAppService = hanetTenantPlaceDatasesAppService;
        }

        public ActionResult Index()
        {
            var model = new HanetTenantPlaceDatasesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases_Create, AppPermissions.Pages_HanetTenantPlaceDatases_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id, int tenantid)
			{
				GetHanetTenantPlaceDatasForEditOutput getHanetTenantPlaceDatasForEditOutput;

				if (id.HasValue){
					getHanetTenantPlaceDatasForEditOutput = await _hanetTenantPlaceDatasesAppService.GetHanetTenantPlaceDatasForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getHanetTenantPlaceDatasForEditOutput = new GetHanetTenantPlaceDatasForEditOutput{
						HanetTenantPlaceDatas = new CreateOrEditHanetTenantPlaceDatasDto()
					};
				}

				getHanetTenantPlaceDatasForEditOutput.HanetTenantPlaceDatas.tenantId = tenantid;
				var viewModel = new CreateOrEditHanetTenantPlaceDatasModalViewModel()
				{
					HanetTenantPlaceDatas = getHanetTenantPlaceDatasForEditOutput.HanetTenantPlaceDatas,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewHanetTenantPlaceDatasModal(int id)
        {
			var getHanetTenantPlaceDatasForViewDto = await _hanetTenantPlaceDatasesAppService.GetHanetTenantPlaceDatasForView(id);

            var model = new HanetTenantPlaceDatasViewModel()
            {
                HanetTenantPlaceDatas = getHanetTenantPlaceDatasForViewDto.HanetTenantPlaceDatas
            };

            return PartialView("_ViewHanetTenantPlaceDatasModal", model);
        }


    }
}