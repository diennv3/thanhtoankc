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
	[Table("BacSiChuyenKhoas")]
    [Audited]
    public class BacSiChuyenKhoa : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			


		public virtual long UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int ChuyenKhoaId { get; set; }
		
        [ForeignKey("ChuyenKhoaId")]
		public ChuyenKhoa ChuyenKhoaFk { get; set; }
		
    }
}