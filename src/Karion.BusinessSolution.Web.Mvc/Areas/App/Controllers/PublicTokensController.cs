using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.PublicTokens;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PublicTokens)]
    public class PublicTokensController : BusinessSolutionControllerBase
    {
        private readonly IPublicTokensAppService _publicTokensAppService;

        public PublicTokensController(IPublicTokensAppService publicTokensAppService)
        {
            _publicTokensAppService = publicTokensAppService;
        }

        public ActionResult Index()
        {
            var model = new PublicTokensViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PublicTokens_Create, AppPermissions.Pages_PublicTokens_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetPublicTokenForEditOutput getPublicTokenForEditOutput;

				if (id.HasValue){
					getPublicTokenForEditOutput = await _publicTokensAppService.GetPublicTokenForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPublicTokenForEditOutput = new GetPublicTokenForEditOutput{
						PublicToken = new CreateOrEditPublicTokenDto()
					};
				getPublicTokenForEditOutput.PublicToken.TimeSet = DateTime.Now;
				getPublicTokenForEditOutput.PublicToken.TimeExpire = DateTime.Now;
				}

				var viewModel = new CreateOrEditPublicTokenModalViewModel()
				{
					PublicToken = getPublicTokenForEditOutput.PublicToken,
					NguoiBenhUserName = getPublicTokenForEditOutput.NguoiBenhUserName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPublicTokenModal(long id)
        {
			var getPublicTokenForViewDto = await _publicTokensAppService.GetPublicTokenForView(id);

            var model = new PublicTokenViewModel()
            {
                PublicToken = getPublicTokenForViewDto.PublicToken
                , NguoiBenhUserName = getPublicTokenForViewDto.NguoiBenhUserName 

            };

            return PartialView("_ViewPublicTokenModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PublicTokens_Create, AppPermissions.Pages_PublicTokens_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new PublicTokenNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PublicTokenNguoiBenhLookupTableModal", viewModel);
        }

    }
}