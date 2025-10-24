using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("Shifts")]
    public class Shift : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(ShiftConsts.MaxCodeLength, MinimumLength = ShiftConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(ShiftConsts.MaxNameLength, MinimumLength = ShiftConsts.MinNameLength)]
		public virtual string Name { get; set; }
		[Column(TypeName = "time")]
		public virtual TimeSpan StartTime { get; set; }

		[Column(TypeName = "time")]
		public virtual TimeSpan EndTime { get; set; }

		public virtual int? WorkDaysMask { get; set; }

		public virtual int ToleranceMinutes { get; set; } = 15;

		public virtual bool IsActive { get; set; } = true;

		public virtual string Description { get; set; }
		

    }
}