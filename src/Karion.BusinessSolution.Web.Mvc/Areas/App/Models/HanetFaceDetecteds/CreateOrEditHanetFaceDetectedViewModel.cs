using Karion.BusinessSolution.HanetTenant.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.HanetFaceDetecteds
{
    public class CreateOrEditHanetFaceDetectedModalViewModel
    {
       public CreateOrEditHanetFaceDetectedDto HanetFaceDetected { get; set; }

	   
       
	   public bool IsEditMode => HanetFaceDetected.Id.HasValue;
    }
}