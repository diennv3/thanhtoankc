using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.TechberConfigures;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.TBHostConfigure;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TechberConfigures)]
    public class TechberConfiguresController : BusinessSolutionControllerBase
    {
        private readonly ITechberConfiguresAppService _techberConfiguresAppService;

        public TechberConfiguresController(ITechberConfiguresAppService techberConfiguresAppService)
        {
            _techberConfiguresAppService = techberConfiguresAppService;
        }

        public ActionResult Index()
        {
            var model = new TechberConfiguresViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       
[AbpMvcAuthorize(AppPermissions.Pages_TechberConfigures_Create, AppPermissions.Pages_TechberConfigures_Edit)]
			public async Task<ActionResult> CreateOrEdit(int? id)
			{
				GetTechberConfigureForEditOutput getTechberConfigureForEditOutput;

				if (id.HasValue){
					getTechberConfigureForEditOutput = await _techberConfiguresAppService.GetTechberConfigureForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getTechberConfigureForEditOutput = new GetTechberConfigureForEditOutput{
						TechberConfigure = new CreateOrEditTechberConfigureDto()
					};
				}

				var viewModel = new CreateOrEditTechberConfigureViewModel()
				{
					TechberConfigure = getTechberConfigureForEditOutput.TechberConfigure,                
				};

				return View(viewModel);
			}
			

        public async Task<ActionResult> ViewTechberConfigure(int id)
        {
			var getTechberConfigureForViewDto = await _techberConfiguresAppService.GetTechberConfigureForView(id);

            var model = new TechberConfigureViewModel()
            {
                TechberConfigure = getTechberConfigureForViewDto.TechberConfigure
            };

            return View(model);
        }


    }
}