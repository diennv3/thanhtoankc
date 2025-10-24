using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.GiaDichVus;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_GiaDichVus)]
    public class GiaDichVusController : BusinessSolutionControllerBase
    {
        private readonly IGiaDichVusAppService _giaDichVusAppService;

        public GiaDichVusController(IGiaDichVusAppService giaDichVusAppService)
        {
            _giaDichVusAppService = giaDichVusAppService;
        }

        public ActionResult Index()
        {
            var model = new GiaDichVusViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_GiaDichVus_Create, AppPermissions.Pages_GiaDichVus_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetGiaDichVuForEditOutput getGiaDichVuForEditOutput;

				if (id.HasValue){
					getGiaDichVuForEditOutput = await _giaDichVusAppService.GetGiaDichVuForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getGiaDichVuForEditOutput = new GetGiaDichVuForEditOutput{
						GiaDichVu = new CreateOrEditGiaDichVuDto()
					};
				getGiaDichVuForEditOutput.GiaDichVu.NgayApDung = DateTime.Now;
				}

				var viewModel = new CreateOrEditGiaDichVuModalViewModel()
				{
					GiaDichVu = getGiaDichVuForEditOutput.GiaDichVu,
					DichVuTen = getGiaDichVuForEditOutput.DichVuTen,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewGiaDichVuModal(int id)
        {
			var getGiaDichVuForViewDto = await _giaDichVusAppService.GetGiaDichVuForView(id);

            var model = new GiaDichVuViewModel()
            {
                GiaDichVu = getGiaDichVuForViewDto.GiaDichVu
                , DichVuTen = getGiaDichVuForViewDto.DichVuTen 

            };

            return PartialView("_ViewGiaDichVuModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_GiaDichVus_Create, AppPermissions.Pages_GiaDichVus_Edit)]
        public PartialViewResult DichVuLookupTableModal(int? id, string displayName)
        {
            var viewModel = new GiaDichVuDichVuLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_GiaDichVuDichVuLookupTableModal", viewModel);
        }

    }
}