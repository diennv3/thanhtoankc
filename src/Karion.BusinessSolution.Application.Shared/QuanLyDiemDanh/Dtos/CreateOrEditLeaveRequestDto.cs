using Karion.BusinessSolution.QuanLyDiemDanh;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class CreateOrEditLeaveRequestDto : EntityDto<int?>
    {
        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public LeaveRequestType Type { get; set; } = LeaveRequestType.FullDay;

        public HalfDayPart? HalfDayPart { get; set; }

        public double? DurationHours { get; set; }

        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

        public string Reason { get; set; }

        public bool IsForEarlyLeave { get; set; }

        public bool IsForLateArrival { get; set; }

        public int? NguoiBenhId { get; set; }
        
        public int? AllowedLateMinutes { get; set; }
        public int? AllowedEarlyMinutes { get; set; }

    }
}