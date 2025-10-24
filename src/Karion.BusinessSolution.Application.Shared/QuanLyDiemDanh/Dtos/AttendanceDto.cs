
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class AttendanceDto : EntityDto
    {
		public double CheckInLatitude { get; set; }

		public double CheckInLongitude { get; set; }

		public bool IsCheckInFaceMatched { get; set; }

		public bool IsWithinLocation { get; set; }

		public string CheckInDeviceInfo { get; set; }

		public string PhotoPath { get; set; }

		public double CheckInFaceMatchPercentage { get; set; }

		public DateTime CheckIn { get; set; }

		public bool IsLateCheckIn { get; set; }

		public bool IsOvertime { get; set; }

		public DateTime? OvertimeStart { get; set; }

		public DateTime? OvertimeEnd { get; set; }


		 public int? NguoiBenhId { get; set; }

		 
    }
}