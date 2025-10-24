using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class GetAllLeaveRequestsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxStartDateTimeFilter { get; set; }
		public DateTime? MinStartDateTimeFilter { get; set; }

		public DateTime? MaxEndDateTimeFilter { get; set; }
		public DateTime? MinEndDateTimeFilter { get; set; }

		public string LeaveTypeFilter { get; set; }

		public int? StatusFilter { get; set; }

		public string ReasonFilter { get; set; }

		public int IsForEarlyLeaveFilter { get; set; }

		public int IsForLateArrivalFilter { get; set; }


		 public string NguoiBenhUserNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 		 public string ShiftNameFilter { get; set; }

		 
    }
}