using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.ThongTinBacSiMoRongs;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs)]
    public class ThongTinBacSiMoRongsController : BusinessSolutionControllerBase
    {
        private readonly IThongTinBacSiMoRongsAppService _thongTinBacSiMoRongsAppService;

        public ThongTinBacSiMoRongsController(IThongTinBacSiMoRongsAppService thongTinBacSiMoRongsAppService)
        {
            _thongTinBacSiMoRongsAppService = thongTinBacSiMoRongsAppService;
        }

        public ActionResult Index()
        {
            var model = new ThongTinBacSiMoRongsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs_Create, AppPermissions.Pages_ThongTinBacSiMoRongs_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetThongTinBacSiMoRongForEditOutput getThongTinBacSiMoRongForEditOutput;

				if (id.HasValue){
					getThongTinBacSiMoRongForEditOutput = await _thongTinBacSiMoRongsAppService.GetThongTinBacSiMoRongForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getThongTinBacSiMoRongForEditOutput = new GetThongTinBacSiMoRongForEditOutput{
						ThongTinBacSiMoRong = new CreateOrEditThongTinBacSiMoRongDto()
					};
				}

				var viewModel = new CreateOrEditThongTinBacSiMoRongModalViewModel()
				{
					ThongTinBacSiMoRong = getThongTinBacSiMoRongForEditOutput.ThongTinBacSiMoRong,
					UserName = getThongTinBacSiMoRongForEditOutput.UserName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewThongTinBacSiMoRongModal(int id)
        {
			var getThongTinBacSiMoRongForViewDto = await _thongTinBacSiMoRongsAppService.GetThongTinBacSiMoRongForView(id);

            var model = new ThongTinBacSiMoRongViewModel()
            {
                ThongTinBacSiMoRong = getThongTinBacSiMoRongForViewDto.ThongTinBacSiMoRong
                , UserName = getThongTinBacSiMoRongForViewDto.UserName 

            };

            return PartialView("_ViewThongTinBacSiMoRongModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs_Create, AppPermissions.Pages_ThongTinBacSiMoRongs_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ThongTinBacSiMoRongUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ThongTinBacSiMoRongUserLookupTableModal", viewModel);
        }

    }
}