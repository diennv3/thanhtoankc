using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.LeaveRequests
{
    public class CreateOrEditLeaveRequestModalViewModel
    {
       public CreateOrEditLeaveRequestDto LeaveRequest { get; set; }

	   		public string NguoiBenhUserName { get; set;}

		public string UserName { get; set;}

		public string ShiftName { get; set;}


       
	   public bool IsEditMode => LeaveRequest.Id.HasValue;
    }
}