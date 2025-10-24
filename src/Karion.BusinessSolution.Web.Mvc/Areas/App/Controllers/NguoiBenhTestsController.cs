using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.NguoiBenhTests;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.NguoiBenhTestNS;
using Karion.BusinessSolution.NguoiBenhTestNS.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhTests)]
    public class NguoiBenhTestsController : BusinessSolutionControllerBase
    {
        private readonly INguoiBenhTestsAppService _nguoiBenhTestsAppService;

        public NguoiBenhTestsController(INguoiBenhTestsAppService nguoiBenhTestsAppService)
        {
            _nguoiBenhTestsAppService = nguoiBenhTestsAppService;
        }

        public ActionResult Index()
        {
            var model = new NguoiBenhTestsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhTests_Create, AppPermissions.Pages_NguoiBenhTests_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetNguoiBenhTestForEditOutput getNguoiBenhTestForEditOutput;

				if (id.HasValue){
					getNguoiBenhTestForEditOutput = await _nguoiBenhTestsAppService.GetNguoiBenhTestForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getNguoiBenhTestForEditOutput = new GetNguoiBenhTestForEditOutput{
						NguoiBenhTest = new CreateOrEditNguoiBenhTestDto()
					};
				}

				var viewModel = new CreateOrEditNguoiBenhTestModalViewModel()
				{
					NguoiBenhTest = getNguoiBenhTestForEditOutput.NguoiBenhTest,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewNguoiBenhTestModal(int id)
        {
			var getNguoiBenhTestForViewDto = await _nguoiBenhTestsAppService.GetNguoiBenhTestForView(id);

            var model = new NguoiBenhTestViewModel()
            {
                NguoiBenhTest = getNguoiBenhTestForViewDto.NguoiBenhTest
            };

            return PartialView("_ViewNguoiBenhTestModal", model);
        }


    }
}