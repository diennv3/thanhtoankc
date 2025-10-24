using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.Shifts;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Shifts)]
    public class ShiftsController : BusinessSolutionControllerBase
    {
        private readonly IShiftsAppService _shiftsAppService;

        public ShiftsController(IShiftsAppService shiftsAppService)
        {
            _shiftsAppService = shiftsAppService;
        }

        public ActionResult Index()
        {
            var model = new ShiftsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Shifts_Create, AppPermissions.Pages_Shifts_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetShiftForEditOutput getShiftForEditOutput;

				if (id.HasValue){
					getShiftForEditOutput = await _shiftsAppService.GetShiftForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getShiftForEditOutput = new GetShiftForEditOutput{
						Shift = new CreateOrEditShiftDto()
					};
					getShiftForEditOutput.Shift.StartTime = DateTime.Now.TimeOfDay;
					getShiftForEditOutput.Shift.EndTime = DateTime.Now.TimeOfDay;
				}

				var viewModel = new CreateOrEditShiftModalViewModel()
				{
					Shift = getShiftForEditOutput.Shift,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewShiftModal(int id)
        {
			var getShiftForViewDto = await _shiftsAppService.GetShiftForView(id);

            var model = new ShiftViewModel()
            {
                Shift = getShiftForViewDto.Shift
            };

            return PartialView("_ViewShiftModal", model);
        }


    }
}