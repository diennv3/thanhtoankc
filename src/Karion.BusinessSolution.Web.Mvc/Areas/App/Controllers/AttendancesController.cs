using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.Attendances;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDiemDanh;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Attendances)]
    public class AttendancesController : BusinessSolutionControllerBase
    {
        private readonly IAttendancesAppService _attendancesAppService;

        public AttendancesController(IAttendancesAppService attendancesAppService)
        {
            _attendancesAppService = attendancesAppService;
        }

        public ActionResult Index()
        {
            var model = new AttendancesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Attendances_Create, AppPermissions.Pages_Attendances_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetAttendanceForEditOutput getAttendanceForEditOutput;

				if (id.HasValue){
					getAttendanceForEditOutput = await _attendancesAppService.GetAttendanceForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getAttendanceForEditOutput = new GetAttendanceForEditOutput{
						Attendance = new CreateOrEditAttendanceDto()
					};
				getAttendanceForEditOutput.Attendance.CheckIn = DateTime.Now;
				getAttendanceForEditOutput.Attendance.OvertimeStart = DateTime.Now;
				getAttendanceForEditOutput.Attendance.OvertimeEnd = DateTime.Now;
				}

				var viewModel = new CreateOrEditAttendanceModalViewModel()
				{
					Attendance = getAttendanceForEditOutput.Attendance,
					NguoiBenhUserName = getAttendanceForEditOutput.NguoiBenhUserName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewAttendanceModal(int id)
        {
			var getAttendanceForViewDto = await _attendancesAppService.GetAttendanceForView(id);

            var model = new AttendanceViewModel()
            {
                Attendance = getAttendanceForViewDto.Attendance
                , NguoiBenhName = getAttendanceForViewDto.NguoiBenhName 

            };

            return PartialView("_ViewAttendanceModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Attendances_Create, AppPermissions.Pages_Attendances_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new AttendanceNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_AttendanceNguoiBenhLookupTableModal", viewModel);
        }

    }
}