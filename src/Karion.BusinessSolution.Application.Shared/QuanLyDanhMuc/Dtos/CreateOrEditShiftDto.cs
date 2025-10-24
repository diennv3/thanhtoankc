
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditShiftDto : EntityDto<int?>
    {

		[StringLength(ShiftConsts.MaxCodeLength, MinimumLength = ShiftConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(ShiftConsts.MaxNameLength, MinimumLength = ShiftConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		
		
		public int? WorkDaysMask { get; set; }
		
		
		public int ToleranceMinutes { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		public string Description { get; set; }
		
		

    }
}