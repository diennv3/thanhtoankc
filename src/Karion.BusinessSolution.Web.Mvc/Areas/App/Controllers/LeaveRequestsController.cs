using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.LeaveRequests;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDiemDanh;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LeaveRequests)]
    public class LeaveRequestsController : BusinessSolutionControllerBase
    {
        private readonly ILeaveRequestsAppService _leaveRequestsAppService;

        public LeaveRequestsController(ILeaveRequestsAppService leaveRequestsAppService)
        {
            _leaveRequestsAppService = leaveRequestsAppService;
        }

        public ActionResult Index()
        {
            var model = new LeaveRequestsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_LeaveRequests_Create, AppPermissions.Pages_LeaveRequests_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetLeaveRequestForEditOutput getLeaveRequestForEditOutput;

				if (id.HasValue){
					getLeaveRequestForEditOutput = await _leaveRequestsAppService.GetLeaveRequestForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getLeaveRequestForEditOutput = new GetLeaveRequestForEditOutput{
						LeaveRequest = new CreateOrEditLeaveRequestDto()
					};
				getLeaveRequestForEditOutput.LeaveRequest.StartDateTime = DateTime.Now;
				getLeaveRequestForEditOutput.LeaveRequest.EndDateTime = DateTime.Now;
				}

				var viewModel = new CreateOrEditLeaveRequestModalViewModel()
				{
					LeaveRequest = getLeaveRequestForEditOutput.LeaveRequest,
					NguoiBenhUserName = getLeaveRequestForEditOutput.NguoiBenhUserName,
					UserName = getLeaveRequestForEditOutput.UserName,
					ShiftName = getLeaveRequestForEditOutput.ShiftName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewLeaveRequestModal(int id)
        {
			var getLeaveRequestForViewDto = await _leaveRequestsAppService.GetLeaveRequestForView(id);

            var model = new LeaveRequestViewModel()
            {
                LeaveRequest = getLeaveRequestForViewDto.LeaveRequest
                , NguoiBenhUserName = getLeaveRequestForViewDto.NguoiBenhUserName 

                , UserName = getLeaveRequestForViewDto.UserName 

                , ShiftName = getLeaveRequestForViewDto.ShiftName 

            };

            return PartialView("_ViewLeaveRequestModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LeaveRequests_Create, AppPermissions.Pages_LeaveRequests_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new LeaveRequestNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LeaveRequestNguoiBenhLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_LeaveRequests_Create, AppPermissions.Pages_LeaveRequests_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new LeaveRequestUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LeaveRequestUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_LeaveRequests_Create, AppPermissions.Pages_LeaveRequests_Edit)]
        public PartialViewResult ShiftLookupTableModal(int? id, string displayName)
        {
            var viewModel = new LeaveRequestShiftLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LeaveRequestShiftLookupTableModal", viewModel);
        }

    }
}