using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.HanetTenantLogs;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantLogs)]
    public class HanetTenantLogsController : BusinessSolutionControllerBase
    {
        private readonly IHanetTenantLogsAppService _hanetTenantLogsAppService;

        public HanetTenantLogsController(IHanetTenantLogsAppService hanetTenantLogsAppService)
        {
            _hanetTenantLogsAppService = hanetTenantLogsAppService;
        }

        public ActionResult Index()
        {
            var model = new HanetTenantLogsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_HanetTenantLogs_Create, AppPermissions.Pages_HanetTenantLogs_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetHanetTenantLogForEditOutput getHanetTenantLogForEditOutput;

				if (id.HasValue){
					getHanetTenantLogForEditOutput = await _hanetTenantLogsAppService.GetHanetTenantLogForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getHanetTenantLogForEditOutput = new GetHanetTenantLogForEditOutput{
						HanetTenantLog = new CreateOrEditHanetTenantLogDto()
					};
				}

				var viewModel = new CreateOrEditHanetTenantLogModalViewModel()
				{
					HanetTenantLog = getHanetTenantLogForEditOutput.HanetTenantLog,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewHanetTenantLogModal(int id)
        {
			var getHanetTenantLogForViewDto = await _hanetTenantLogsAppService.GetHanetTenantLogForView(id);

            var model = new HanetTenantLogViewModel()
            {
                HanetTenantLog = getHanetTenantLogForViewDto.HanetTenantLog
            };

            return PartialView("_ViewHanetTenantLogModal", model);
        }


    }
}