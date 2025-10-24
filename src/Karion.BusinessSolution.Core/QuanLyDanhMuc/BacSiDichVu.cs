using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("BacSiDichVus")]
    [Audited]
    public class BacSiDichVu : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			


		public virtual long UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int DichVuId { get; set; }
		
        [ForeignKey("DichVuId")]
		public DichVu DichVuFk { get; set; }
		
    }
}