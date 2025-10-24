using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("ChuyenKhoas")]
    [Audited]
    public class ChuyenKhoa : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ChuyenKhoaConsts.MaxTenLength, MinimumLength = ChuyenKhoaConsts.MinTenLength)]
		public virtual string Ten { get; set; }
		
		public virtual string MoTa { get; set; }
		

    }
}