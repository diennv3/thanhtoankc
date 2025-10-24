using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Shifts
{
    public class CreateOrEditShiftModalViewModel
    {
       public CreateOrEditShiftDto Shift { get; set; }

	   
       
	   public bool IsEditMode => Shift.Id.HasValue;
    }
}