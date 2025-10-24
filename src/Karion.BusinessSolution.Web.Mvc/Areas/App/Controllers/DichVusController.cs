using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.DichVus;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DichVus)]
    public class DichVusController : BusinessSolutionControllerBase
    {
        private readonly IDichVusAppService _dichVusAppService;

        public DichVusController(IDichVusAppService dichVusAppService)
        {
            _dichVusAppService = dichVusAppService;
        }

        public ActionResult Index()
        {
            var model = new DichVusViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_DichVus_Create, AppPermissions.Pages_DichVus_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetDichVuForEditOutput getDichVuForEditOutput;

				if (id.HasValue){
					getDichVuForEditOutput = await _dichVusAppService.GetDichVuForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getDichVuForEditOutput = new GetDichVuForEditOutput{
						DichVu = new CreateOrEditDichVuDto()
					};
				}

				var viewModel = new CreateOrEditDichVuModalViewModel()
				{
					DichVu = getDichVuForEditOutput.DichVu,
					ChuyenKhoaTen = getDichVuForEditOutput.ChuyenKhoaTen,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewDichVuModal(int id)
        {
			var getDichVuForViewDto = await _dichVusAppService.GetDichVuForView(id);

            var model = new DichVuViewModel()
            {
                DichVu = getDichVuForViewDto.DichVu
                , ChuyenKhoaTen = getDichVuForViewDto.ChuyenKhoaTen 

            };

            return PartialView("_ViewDichVuModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DichVus_Create, AppPermissions.Pages_DichVus_Edit)]
        public PartialViewResult ChuyenKhoaLookupTableModal(int? id, string displayName)
        {
            var viewModel = new DichVuChuyenKhoaLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DichVuChuyenKhoaLookupTableModal", viewModel);
        }

    }
}