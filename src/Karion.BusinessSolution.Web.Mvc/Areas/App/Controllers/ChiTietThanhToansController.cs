using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.ChiTietThanhToans;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThanhToans)]
    public class ChiTietThanhToansController : BusinessSolutionControllerBase
    {
        private readonly IChiTietThanhToansAppService _chiTietThanhToansAppService;

        public ChiTietThanhToansController(IChiTietThanhToansAppService chiTietThanhToansAppService)
        {
            _chiTietThanhToansAppService = chiTietThanhToansAppService;
        }

        public ActionResult Index()
        {
            var model = new ChiTietThanhToansViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThanhToans_Create, AppPermissions.Pages_ChiTietThanhToans_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetChiTietThanhToanForEditOutput getChiTietThanhToanForEditOutput;

				if (id.HasValue){
					getChiTietThanhToanForEditOutput = await _chiTietThanhToansAppService.GetChiTietThanhToanForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getChiTietThanhToanForEditOutput = new GetChiTietThanhToanForEditOutput{
						ChiTietThanhToan = new CreateOrEditChiTietThanhToanDto()
					};
				getChiTietThanhToanForEditOutput.ChiTietThanhToan.NgayThanhToan = DateTime.Now;
				}

				var viewModel = new CreateOrEditChiTietThanhToanModalViewModel()
				{
					ChiTietThanhToan = getChiTietThanhToanForEditOutput.ChiTietThanhToan,
					LichHenKhamMoTaTrieuChung = getChiTietThanhToanForEditOutput.LichHenKhamMoTaTrieuChung,
					NguoiBenhUserName = getChiTietThanhToanForEditOutput.NguoiBenhUserName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewChiTietThanhToanModal(int id)
        {
			var getChiTietThanhToanForViewDto = await _chiTietThanhToansAppService.GetChiTietThanhToanForView(id);

            var model = new ChiTietThanhToanViewModel()
            {
                ChiTietThanhToan = getChiTietThanhToanForViewDto.ChiTietThanhToan
                , LichHenKhamMoTaTrieuChung = getChiTietThanhToanForViewDto.LichHenKhamMoTaTrieuChung 

                , NguoiBenhUserName = getChiTietThanhToanForViewDto.NguoiBenhUserName 

            };

            return PartialView("_ViewChiTietThanhToanModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThanhToans_Create, AppPermissions.Pages_ChiTietThanhToans_Edit)]
        public PartialViewResult LichHenKhamLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ChiTietThanhToanLichHenKhamLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ChiTietThanhToanLichHenKhamLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThanhToans_Create, AppPermissions.Pages_ChiTietThanhToans_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ChiTietThanhToanNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ChiTietThanhToanNguoiBenhLookupTableModal", viewModel);
        }

    }
}