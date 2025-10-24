using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.BacSiDichVus;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_BacSiDichVus)]
    public class BacSiDichVusController : BusinessSolutionControllerBase
    {
        private readonly IBacSiDichVusAppService _bacSiDichVusAppService;

        public BacSiDichVusController(IBacSiDichVusAppService bacSiDichVusAppService)
        {
            _bacSiDichVusAppService = bacSiDichVusAppService;
        }

        public ActionResult Index()
        {
            var model = new BacSiDichVusViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_BacSiDichVus_Create, AppPermissions.Pages_BacSiDichVus_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetBacSiDichVuForEditOutput getBacSiDichVuForEditOutput;

				if (id.HasValue){
					getBacSiDichVuForEditOutput = await _bacSiDichVusAppService.GetBacSiDichVuForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getBacSiDichVuForEditOutput = new GetBacSiDichVuForEditOutput{
						BacSiDichVu = new CreateOrEditBacSiDichVuDto()
					};
				}

				var viewModel = new CreateOrEditBacSiDichVuModalViewModel()
				{
					BacSiDichVu = getBacSiDichVuForEditOutput.BacSiDichVu,
					UserName = getBacSiDichVuForEditOutput.UserName,
					DichVuTen = getBacSiDichVuForEditOutput.DichVuTen,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewBacSiDichVuModal(int id)
        {
			var getBacSiDichVuForViewDto = await _bacSiDichVusAppService.GetBacSiDichVuForView(id);

            var model = new BacSiDichVuViewModel()
            {
                BacSiDichVu = getBacSiDichVuForViewDto.BacSiDichVu
                , UserName = getBacSiDichVuForViewDto.UserName 

                , DichVuTen = getBacSiDichVuForViewDto.DichVuTen 

            };

            return PartialView("_ViewBacSiDichVuModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BacSiDichVus_Create, AppPermissions.Pages_BacSiDichVus_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BacSiDichVuUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BacSiDichVuUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_BacSiDichVus_Create, AppPermissions.Pages_BacSiDichVus_Edit)]
        public PartialViewResult DichVuLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BacSiDichVuDichVuLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BacSiDichVuDichVuLookupTableModal", viewModel);
        }

    }
}