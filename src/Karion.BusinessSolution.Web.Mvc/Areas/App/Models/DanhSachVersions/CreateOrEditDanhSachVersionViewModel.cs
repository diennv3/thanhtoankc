using Karion.BusinessSolution.VersionControl.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.DanhSachVersions
{
    public class CreateOrEditDanhSachVersionModalViewModel
    {
       public CreateOrEditDanhSachVersionDto DanhSachVersion { get; set; }

	   
       
	   public bool IsEditMode => DanhSachVersion.Id.HasValue;
    }
}