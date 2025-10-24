using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("ThongTinDonVies")]
    [Audited]
    public class ThongTinDonVi : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ThongTinDonViConsts.MaxKeyLength, MinimumLength = ThongTinDonViConsts.MinKeyLength)]
		public virtual string Key { get; set; }
		
		public virtual string Value { get; set; }
		

    }
}