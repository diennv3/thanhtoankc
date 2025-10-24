using Karion.BusinessSolution.VersionControl.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Versions
{
    public class CreateOrEditVersionModalViewModel
    {
       public CreateOrEditVersionDto Version { get; set; }

	   
       
	   public bool IsEditMode => Version.Id.HasValue;
    }
}