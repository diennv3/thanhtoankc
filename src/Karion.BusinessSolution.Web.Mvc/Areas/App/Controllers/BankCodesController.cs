using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.BankCodes;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_BankCodes)]
    public class BankCodesController : BusinessSolutionControllerBase
    {
        private readonly IBankCodesAppService _bankCodesAppService;

        public BankCodesController(IBankCodesAppService bankCodesAppService)
        {
            _bankCodesAppService = bankCodesAppService;
        }

        public ActionResult Index()
        {
            var model = new BankCodesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_BankCodes_Create, AppPermissions.Pages_BankCodes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetBankCodeForEditOutput getBankCodeForEditOutput;

				if (id.HasValue){
					getBankCodeForEditOutput = await _bankCodesAppService.GetBankCodeForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getBankCodeForEditOutput = new GetBankCodeForEditOutput{
						BankCode = new CreateOrEditBankCodeDto()
					};
				}

				var viewModel = new CreateOrEditBankCodeModalViewModel()
				{
					BankCode = getBankCodeForEditOutput.BankCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewBankCodeModal(int id)
        {
			var getBankCodeForViewDto = await _bankCodesAppService.GetBankCodeForView(id);

            var model = new BankCodeViewModel()
            {
                BankCode = getBankCodeForViewDto.BankCode
            };

            return PartialView("_ViewBankCodeModal", model);
        }


    }
}