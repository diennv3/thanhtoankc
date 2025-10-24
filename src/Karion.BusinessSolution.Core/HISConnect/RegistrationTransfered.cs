using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Karion.BusinessSolution.HISConnect
{
	[Table("RegistrationTransfereds")]
    public class RegistrationTransfered : Entity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual DateTime ApprovedDate { get; set; }
		
		public virtual DateTime RegistrationDate { get; set; }
		

		public virtual int? LichHenKhamId { get; set; }
		
        [ForeignKey("LichHenKhamId")]
		public LichHenKham LichHenKhamFk { get; set; }
		
		public virtual int? NguoiBenhId { get; set; }
		
        [ForeignKey("NguoiBenhId")]
		public NguoiBenh NguoiBenhFk { get; set; }
		
    }
}