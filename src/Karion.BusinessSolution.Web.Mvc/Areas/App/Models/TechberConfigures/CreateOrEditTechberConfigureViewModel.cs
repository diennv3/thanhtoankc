using Karion.BusinessSolution.TBHostConfigure.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.TechberConfigures
{
    public class CreateOrEditTechberConfigureViewModel
    {
       public CreateOrEditTechberConfigureDto TechberConfigure { get; set; }

	   
       
	   public bool IsEditMode => TechberConfigure.Id.HasValue;
    }
}