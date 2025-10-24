using Karion.BusinessSolution.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("ThongTinBacSiMoRongs")]
    [Audited]
    public class ThongTinBacSiMoRong : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Image { get; set; }
		
		public virtual string TieuSu { get; set; }
		
		public virtual string ChucDanh { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		public virtual string ImageThumbnail { get; set; }
    }
}