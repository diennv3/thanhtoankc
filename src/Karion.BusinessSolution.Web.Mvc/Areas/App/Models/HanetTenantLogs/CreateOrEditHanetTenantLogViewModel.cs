using Karion.BusinessSolution.HanetTenant.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.HanetTenantLogs
{
    public class CreateOrEditHanetTenantLogModalViewModel
    {
       public CreateOrEditHanetTenantLogDto HanetTenantLog { get; set; }

	   
       
	   public bool IsEditMode => HanetTenantLog.Id.HasValue;
    }
}