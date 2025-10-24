using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("PublicTokens")]
    [Audited]
    public class PublicToken : FullAuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual DateTime TimeSet { get; set; }
		
		public virtual DateTime TimeExpire { get; set; }
		
		public virtual string Token { get; set; }
		
		public virtual string PrivateKey { get; set; }
		
		[StringLength(PublicTokenConsts.MaxDeviceVerificationCodeLength, MinimumLength = PublicTokenConsts.MinDeviceVerificationCodeLength)]
		public virtual string DeviceVerificationCode { get; set; }
		
		[StringLength(PublicTokenConsts.MaxLastAccessDeviceVerificationCodeLength, MinimumLength = PublicTokenConsts.MinLastAccessDeviceVerificationCodeLength)]
		public virtual string LastAccessDeviceVerificationCode { get; set; }
		
		public virtual bool IsTokenLocked { get; set; }
		

		public virtual int? NguoiBenhId { get; set; }
		
        [ForeignKey("NguoiBenhId")]
		public NguoiBenh NguoiBenhFk { get; set; }
		
    }
}