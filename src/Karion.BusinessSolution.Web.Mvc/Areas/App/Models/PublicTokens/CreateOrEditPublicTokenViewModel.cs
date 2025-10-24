using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.PublicTokens
{
    public class CreateOrEditPublicTokenModalViewModel
    {
       public CreateOrEditPublicTokenDto PublicToken { get; set; }

	   		public string NguoiBenhUserName { get; set;}


       
	   public bool IsEditMode => PublicToken.Id.HasValue;
    }
}