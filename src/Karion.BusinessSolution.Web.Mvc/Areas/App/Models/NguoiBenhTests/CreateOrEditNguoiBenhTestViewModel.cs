using Karion.BusinessSolution.NguoiBenhTestNS.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.NguoiBenhTests
{
    public class CreateOrEditNguoiBenhTestModalViewModel
    {
       public CreateOrEditNguoiBenhTestDto NguoiBenhTest { get; set; }

	   
       
	   public bool IsEditMode => NguoiBenhTest.Id.HasValue;
    }
}