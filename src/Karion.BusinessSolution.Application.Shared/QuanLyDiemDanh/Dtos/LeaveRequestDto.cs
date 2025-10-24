using Karion.BusinessSolution.QuanLyDiemDanh;

using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class LeaveRequestDto : EntityDto
    {
		public DateTime StartDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public LeaveRequestType Type { get; set; } = LeaveRequestType.FullDay;

		public HalfDayPart? HalfDayPart { get; set; }

		public LeaveRequestStatus Status { get; set; }

		public string Reason { get; set; }

		public bool IsForEarlyLeave { get; set; }
		public bool IsForLateArrival { get; set; }

		public int? AllowedLateMinutes { get; set; }
		public int? AllowedEarlyMinutes { get; set; }

		 public int? NguoiBenhId { get; set; }
    }
}