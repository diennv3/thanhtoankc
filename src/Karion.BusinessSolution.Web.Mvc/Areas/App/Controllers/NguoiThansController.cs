using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.NguoiThans;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_NguoiThans)]
    public class NguoiThansController : BusinessSolutionControllerBase
    {
        private readonly INguoiThansAppService _nguoiThansAppService;

        public NguoiThansController(INguoiThansAppService nguoiThansAppService)
        {
            _nguoiThansAppService = nguoiThansAppService;
        }

        public ActionResult Index()
        {
            var model = new NguoiThansViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

		public async Task<PartialViewResult> GetModalNguoiThan(int? nguoithanid)
		{
			GetNguoiThanForEditOutput getNguoiThanForEditOutput;

			if (nguoithanid.HasValue)
			{
				getNguoiThanForEditOutput = await _nguoiThansAppService.GetNguoiThanForEdit(new EntityDto { Id = (int)nguoithanid });
			}
			else
			{
				getNguoiThanForEditOutput = new GetNguoiThanForEditOutput
				{
					NguoiThan = new CreateOrEditNguoiThanDto()
				};
			}

			var viewModel = new CreateOrEditNguoiThanModalViewModel()
			{
				NguoiThan = getNguoiThanForEditOutput.NguoiThan,
				NguoiBenhHoVaTen = getNguoiThanForEditOutput.NguoiBenhHoVaTen,
			};

			return PartialView("_CreateOrEditModal", viewModel);
		}


		[AbpMvcAuthorize(AppPermissions.Pages_NguoiThans_Create, AppPermissions.Pages_NguoiThans_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetNguoiThanForEditOutput getNguoiThanForEditOutput;

				if (id.HasValue){
					getNguoiThanForEditOutput = await _nguoiThansAppService.GetNguoiThanForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getNguoiThanForEditOutput = new GetNguoiThanForEditOutput{
						NguoiThan = new CreateOrEditNguoiThanDto()
					};
				}

				var viewModel = new CreateOrEditNguoiThanModalViewModel()
				{
					NguoiThan = getNguoiThanForEditOutput.NguoiThan,
					NguoiBenhHoVaTen = getNguoiThanForEditOutput.NguoiBenhHoVaTen,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewNguoiThanModal(int id)
        {
			var getNguoiThanForViewDto = await _nguoiThansAppService.GetNguoiThanForView(id);

            var model = new NguoiThanViewModel()
            {
                NguoiThan = getNguoiThanForViewDto.NguoiThan
                , NguoiBenhHoVaTen = getNguoiThanForViewDto.NguoiBenhHoVaTen 

            };

            return PartialView("_ViewNguoiThanModal", model);
        }

		public async Task<PartialViewResult> ViewDanhSachNguoiThanModal(int id)
		{
			

			var model = new ListNguoiThanInputViewModel()
			{
				NguoiBenhId=id
			};

			return PartialView("_ViewListNguoiThanModal", model);
		}

		[AbpMvcAuthorize(AppPermissions.Pages_NguoiThans_Create, AppPermissions.Pages_NguoiThans_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new NguoiThanNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_NguoiThanNguoiBenhLookupTableModal", viewModel);
        }

    }
}