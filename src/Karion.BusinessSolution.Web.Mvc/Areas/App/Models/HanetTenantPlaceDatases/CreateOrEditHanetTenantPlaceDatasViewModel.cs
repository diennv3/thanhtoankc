using Karion.BusinessSolution.HanetTenant.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.HanetTenantPlaceDatases
{
    public class CreateOrEditHanetTenantPlaceDatasModalViewModel
    {
       public CreateOrEditHanetTenantPlaceDatasDto HanetTenantPlaceDatas { get; set; }

	   
       
	   public bool IsEditMode => HanetTenantPlaceDatas.Id.HasValue;
    }
}