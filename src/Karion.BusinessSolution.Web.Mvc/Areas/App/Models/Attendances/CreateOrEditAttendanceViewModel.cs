using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Attendances
{
    public class CreateOrEditAttendanceModalViewModel
    {
       public CreateOrEditAttendanceDto Attendance { get; set; }

	   		public string NguoiBenhUserName { get; set;}


       
	   public bool IsEditMode => Attendance.Id.HasValue;
    }
}