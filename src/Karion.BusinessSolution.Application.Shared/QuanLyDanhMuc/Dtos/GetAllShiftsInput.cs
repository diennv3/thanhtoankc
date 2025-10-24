using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllShiftsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public TimeSpan? MaxStartTimeFilter { get; set; }
		public TimeSpan? MinStartTimeFilter { get; set; }

		public TimeSpan? MaxEndTimeFilter { get; set; }
		public TimeSpan? MinEndTimeFilter { get; set; }

		public int? MaxWorkDaysMaskFilter { get; set; }
		public int? MinWorkDaysMaskFilter { get; set; }

		public int? MaxToleranceMinutesFilter { get; set; }
		public int? MinToleranceMinutesFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}