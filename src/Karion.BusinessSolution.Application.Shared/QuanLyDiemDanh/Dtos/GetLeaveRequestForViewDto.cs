namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class GetLeaveRequestForViewDto
    {
		public LeaveRequestDto LeaveRequest { get; set; }

		public string NguoiBenhUserName { get; set;}

		public string UserName { get; set;}

		public string ShiftName { get; set;}


    }
}