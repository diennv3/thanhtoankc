using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("DichVus")]
    [Audited]
    public class DichVu : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(DichVuConsts.MaxTenLength, MinimumLength = DichVuConsts.MinTenLength)]
		public virtual string Ten { get; set; }
		
		public virtual string MoTa { get; set; }
		

		public virtual int? ChuyenKhoaId { get; set; }
		
        [ForeignKey("ChuyenKhoaId")]
		public ChuyenKhoa ChuyenKhoaFk { get; set; }
		
    }
}