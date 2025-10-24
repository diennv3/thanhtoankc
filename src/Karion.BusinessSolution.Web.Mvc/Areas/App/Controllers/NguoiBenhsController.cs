using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.NguoiBenhs;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhs)]
    public class NguoiBenhsController : BusinessSolutionControllerBase
    {
        private readonly INguoiBenhsAppService _nguoiBenhsAppService;

        public NguoiBenhsController(INguoiBenhsAppService nguoiBenhsAppService)
        {
            _nguoiBenhsAppService = nguoiBenhsAppService;
        }

        public ActionResult Index()
        {
            var model = new NguoiBenhsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhs_Create, AppPermissions.Pages_NguoiBenhs_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetNguoiBenhForEditOutput getNguoiBenhForEditOutput;

				if (id.HasValue){
					getNguoiBenhForEditOutput = await _nguoiBenhsAppService.GetNguoiBenhForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getNguoiBenhForEditOutput = new GetNguoiBenhForEditOutput{
						NguoiBenh = new CreateOrEditNguoiBenhDto()
					};
				getNguoiBenhForEditOutput.NguoiBenh.TokenExpire = DateTime.Now;
				}

				var viewModel = new CreateOrEditNguoiBenhModalViewModel()
				{
					NguoiBenh = getNguoiBenhForEditOutput.NguoiBenh,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewNguoiBenhModal(int id)
        {
			var getNguoiBenhForViewDto = await _nguoiBenhsAppService.GetNguoiBenhForView(id);

            var model = new NguoiBenhViewModel()
            {
                NguoiBenh = getNguoiBenhForViewDto.NguoiBenh
            };

            return PartialView("_ViewNguoiBenhModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_NguoiBenhs_Edit)]
        public PartialViewResult UpdateImageProfileModal(int id)
        {
            // Nếu cần có dữ liệu, truyền vào view
            return PartialView("_UpdateImageProfileModal", id);
        }
    }
}