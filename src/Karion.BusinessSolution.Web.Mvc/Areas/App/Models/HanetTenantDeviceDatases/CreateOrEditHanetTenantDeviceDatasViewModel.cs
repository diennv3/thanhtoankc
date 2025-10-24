using Karion.BusinessSolution.HanetTenant.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.HanetTenantDeviceDatases
{
    public class CreateOrEditHanetTenantDeviceDatasModalViewModel
    {
       public CreateOrEditHanetTenantDeviceDatasDto HanetTenantDeviceDatas { get; set; }

	   		public string HanetTenantPlaceDatasplaceName { get; set;}


       
	   public bool IsEditMode => HanetTenantDeviceDatas.Id.HasValue;
    }
}