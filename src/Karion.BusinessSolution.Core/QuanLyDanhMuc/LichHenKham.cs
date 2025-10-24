using Karion.BusinessSolution.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("LichHenKhams")]
    [Audited]
    public class LichHenKham : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual DateTime NgayHenKham { get; set; }
		
		[Required]
		public virtual string MoTaTrieuChung { get; set; }
		
		public virtual bool IsCoBHYT { get; set; }
		
		public virtual string SoTheBHYT { get; set; }
		
		public virtual string NoiDangKyKCBDauTien { get; set; }
		
		public virtual DateTime? BHYTValidDate { get; set; }
		
		public virtual int PhuongThucThanhToan { get; set; }
		
		public virtual bool IsDaKham { get; set; }
		
		public virtual bool IsDaThanhToan { get; set; }
		
		public virtual DateTime? TimeHoanThanhKham { get; set; }
		
		public virtual DateTime? TimeHoanThanhThanhToan { get; set; }
		
		public virtual string ChiDinhDichVuSerialize { get; set; }
		public virtual string QRString { get; set; }
		
		public virtual int Flag { get; set; }

		public virtual decimal TienThua { get; set; }

		public virtual decimal TongTienDaThanhToan { get; set; }

		public virtual long? BacSiId { get; set; }
		
        [ForeignKey("BacSiId")]
		public User BacSiFk { get; set; }
		
		public virtual long? ThuNganId { get; set; }
		
        [ForeignKey("ThuNganId")]
		public User ThuNganFk { get; set; }
		
		public virtual int? NguoiBenhId { get; set; }
		
        [ForeignKey("NguoiBenhId")]
		public NguoiBenh NguoiBenhFk { get; set; }
		
		public virtual int? NguoiThanId { get; set; }
		
        [ForeignKey("NguoiThanId")]
		public NguoiThan NguoiThanFk { get; set; }
		
		public virtual int? ChuyenKhoaId { get; set; }
		
        [ForeignKey("ChuyenKhoaId")]
		public ChuyenKhoa ChuyenKhoaFk { get; set; }
		public int? KhungKham { get; set; }
		
		public virtual decimal TongChiPhi { get; set; }
		public virtual bool IsTamUng { get; set; }
		public virtual bool IsHISGet { get; set; }
		
    }
}