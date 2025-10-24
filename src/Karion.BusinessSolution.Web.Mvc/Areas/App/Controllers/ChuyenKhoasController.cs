using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.ChuyenKhoas;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ChuyenKhoas)]
    public class ChuyenKhoasController : BusinessSolutionControllerBase
    {
        private readonly IChuyenKhoasAppService _chuyenKhoasAppService;

        public ChuyenKhoasController(IChuyenKhoasAppService chuyenKhoasAppService)
        {
            _chuyenKhoasAppService = chuyenKhoasAppService;
        }

        public ActionResult Index()
        {
            var model = new ChuyenKhoasViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ChuyenKhoas_Create, AppPermissions.Pages_ChuyenKhoas_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetChuyenKhoaForEditOutput getChuyenKhoaForEditOutput;

				if (id.HasValue){
					getChuyenKhoaForEditOutput = await _chuyenKhoasAppService.GetChuyenKhoaForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getChuyenKhoaForEditOutput = new GetChuyenKhoaForEditOutput{
						ChuyenKhoa = new CreateOrEditChuyenKhoaDto()
					};
				}

				var viewModel = new CreateOrEditChuyenKhoaModalViewModel()
				{
					ChuyenKhoa = getChuyenKhoaForEditOutput.ChuyenKhoa,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewChuyenKhoaModal(int id)
        {
			var getChuyenKhoaForViewDto = await _chuyenKhoasAppService.GetChuyenKhoaForView(id);

            var model = new ChuyenKhoaViewModel()
            {
                ChuyenKhoa = getChuyenKhoaForViewDto.ChuyenKhoa
            };

            return PartialView("_ViewChuyenKhoaModal", model);
        }


    }
}