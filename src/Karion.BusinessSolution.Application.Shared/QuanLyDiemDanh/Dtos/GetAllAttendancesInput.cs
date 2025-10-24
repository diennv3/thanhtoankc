using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class GetAllAttendancesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public double? MaxCheckInLatitudeFilter { get; set; }
		public double? MinCheckInLatitudeFilter { get; set; }

		public double? MaxCheckInLongitudeFilter { get; set; }
		public double? MinCheckInLongitudeFilter { get; set; }

		public int IsCheckInFaceMatchedFilter { get; set; }

		public int IsWithinLocationFilter { get; set; }

		public string CheckInDeviceInfoFilter { get; set; }

		public string PhotoPathFilter { get; set; }

		public double? MaxCheckInFaceMatchPercentageFilter { get; set; }
		public double? MinCheckInFaceMatchPercentageFilter { get; set; }

		public DateTime? MaxCheckInFilter { get; set; }
		public DateTime? MinCheckInFilter { get; set; }

		public int IsLateCheckInFilter { get; set; }

		public int IsOvertimeFilter { get; set; }

		public DateTime? MaxOvertimeStartFilter { get; set; }
		public DateTime? MinOvertimeStartFilter { get; set; }

		public DateTime? MaxOvertimeEndFilter { get; set; }
		public DateTime? MinOvertimeEndFilter { get; set; }


		 public string NguoiBenhUserNameFilter { get; set; }

		 
    }
}