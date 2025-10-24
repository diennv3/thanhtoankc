
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class ShiftDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }

		public int? WorkDaysMask { get; set; }

		public int ToleranceMinutes { get; set; }

		public bool IsActive { get; set; }

		public string Description { get; set; }



    }
}