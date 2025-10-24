using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("GiaDichVus")]
    [Audited]
    public class GiaDichVu : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(GiaDichVuConsts.MaxMucGiaLength, MinimumLength = GiaDichVuConsts.MinMucGiaLength)]
		public virtual string MucGia { get; set; }
		
		public virtual string MoTa { get; set; }
		
		public virtual int Gia { get; set; }
		
		public virtual DateTime NgayApDung { get; set; }
		

		public virtual int? DichVuId { get; set; }
		
        [ForeignKey("DichVuId")]
		public DichVu DichVuFk { get; set; }
		
    }
}