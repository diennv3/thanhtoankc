using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("NguoiBenhNotifications")]
    [Audited]
    public class NguoiBenhNotification : FullAuditedEntity<long> 
    {

		public virtual string NoiDungTinNhan { get; set; }
		
		public virtual int TrangThai { get; set; }
		
		public virtual string TieuDe { get; set; }
		
		public virtual DateTime ThoiGianGui { get; set; }
		

		public virtual int NguoiBenhId { get; set; }
		
        [ForeignKey("NguoiBenhId")]
		public NguoiBenh NguoiBenhFk { get; set; }
		
    }
}