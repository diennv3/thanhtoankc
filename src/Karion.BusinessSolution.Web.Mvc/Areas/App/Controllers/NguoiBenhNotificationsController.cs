using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.NguoiBenhNotifications;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhNotifications)]
    public class NguoiBenhNotificationsController : BusinessSolutionControllerBase
    {
        private readonly INguoiBenhNotificationsAppService _nguoiBenhNotificationsAppService;

        public NguoiBenhNotificationsController(INguoiBenhNotificationsAppService nguoiBenhNotificationsAppService)
        {
            _nguoiBenhNotificationsAppService = nguoiBenhNotificationsAppService;
        }

        public ActionResult Index()
        {
            var model = new NguoiBenhNotificationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhNotifications_Create, AppPermissions.Pages_NguoiBenhNotifications_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetNguoiBenhNotificationForEditOutput getNguoiBenhNotificationForEditOutput;

				if (id.HasValue){
					getNguoiBenhNotificationForEditOutput = await _nguoiBenhNotificationsAppService.GetNguoiBenhNotificationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getNguoiBenhNotificationForEditOutput = new GetNguoiBenhNotificationForEditOutput{
						NguoiBenhNotification = new CreateOrEditNguoiBenhNotificationDto()
					};
				getNguoiBenhNotificationForEditOutput.NguoiBenhNotification.ThoiGianGui = DateTime.Now;
				}

				var viewModel = new CreateOrEditNguoiBenhNotificationModalViewModel()
				{
					NguoiBenhNotification = getNguoiBenhNotificationForEditOutput.NguoiBenhNotification,
					NguoiBenhUserName = getNguoiBenhNotificationForEditOutput.NguoiBenhUserName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewNguoiBenhNotificationModal(long id)
        {
			var getNguoiBenhNotificationForViewDto = await _nguoiBenhNotificationsAppService.GetNguoiBenhNotificationForView(id);

            var model = new NguoiBenhNotificationViewModel()
            {
                NguoiBenhNotification = getNguoiBenhNotificationForViewDto.NguoiBenhNotification
                , NguoiBenhUserName = getNguoiBenhNotificationForViewDto.NguoiBenhUserName 

            };

            return PartialView("_ViewNguoiBenhNotificationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhNotifications_Create, AppPermissions.Pages_NguoiBenhNotifications_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new NguoiBenhNotificationNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_NguoiBenhNotificationNguoiBenhLookupTableModal", viewModel);
        }

    }
}