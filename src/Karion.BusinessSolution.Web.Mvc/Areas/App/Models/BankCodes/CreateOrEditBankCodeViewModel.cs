using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.BankCodes
{
    public class CreateOrEditBankCodeModalViewModel
    {
       public CreateOrEditBankCodeDto BankCode { get; set; }

	   
       
	   public bool IsEditMode => BankCode.Id.HasValue;
    }
}