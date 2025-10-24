using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("ChiTietThanhToans")]
    [Audited]
    public class ChiTietThanhToan : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual decimal SoTienThanhToan { get; set; }
		
		public virtual int LoaiThanhToan { get; set; }
		
		public virtual DateTime NgayThanhToan { get; set; }
		

		public virtual int? LichHenKhamId { get; set; }
		
        [ForeignKey("LichHenKhamId")]
		public LichHenKham LichHenKhamFk { get; set; }
		
		public virtual int? NguoiBenhId { get; set; }
		
        [ForeignKey("NguoiBenhId")]
		public NguoiBenh NguoiBenhFk { get; set; }
		
		public virtual string QRString { get; set; }
		public virtual bool TrangThaiThanhToan { get; set; }
		
    }
}