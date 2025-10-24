using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("NguoiBenhs")]
    [Audited]
    public class NguoiBenh : FullAuditedEntity 
    {

		[Required]
		[StringLength(NguoiBenhConsts.MaxHoVaTenLength, MinimumLength = NguoiBenhConsts.MinHoVaTenLength)]
		public virtual string HoVaTen { get; set; }
		
		public virtual int NgaySinh { get; set; }
		
		public virtual string GioiTinh { get; set; }
		
		public virtual string DiaChi { get; set; }
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxUserNameLength, MinimumLength = NguoiBenhConsts.MinUserNameLength)]
		public virtual string UserName { get; set; }
		
		public virtual int AccessFailedCount { get; set; }
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxPhoneNumberLength, MinimumLength = NguoiBenhConsts.MinPhoneNumberLength)]
		public virtual string PhoneNumber { get; set; }
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxEmailAddressLength, MinimumLength = NguoiBenhConsts.MinEmailAddressLength)]
		public virtual string EmailAddress { get; set; }
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxEmailConfirmationCodeLength, MinimumLength = NguoiBenhConsts.MinEmailConfirmationCodeLength)]
		public virtual string EmailConfirmationCode { get; set; }
		
		public virtual bool IsActive { get; set; }
		public virtual bool IsNhanVien { get; set; }
		
		public virtual bool IsEmailConfirmed { get; set; }
		
		public virtual bool IsPhoneNumberConfirmed { get; set; }
		
		[StringLength(NguoiBenhConsts.MaxPasswordResetCodeLength, MinimumLength = NguoiBenhConsts.MinPasswordResetCodeLength)]
		public virtual string PasswordResetCode { get; set; }
		
		public virtual string ProfilePicture { get; set; }
		
		[Required]
		public virtual string Password { get; set; }
		
		public virtual string Token { get; set; }
		
		public virtual DateTime TokenExpire { get; set; }
		
		public virtual int ThangSinh { get; set; }
		
		public virtual int NamSinh { get; set; }
		
		public virtual string SoTheBHYT { get; set; }
		
		public virtual string NoiDkBanDau { get; set; }
		
		public virtual string MaDonViBHXH { get; set; }
		
		public virtual DateTime GiaTriSuDungTuNgay { get; set; }
		
		public virtual DateTime ThoiDiemDuNam { get; set; }
		public virtual string DeviceToken { get; set; }
		
		public virtual int? HanetStatus { get; set; }

		public virtual string ErrorMessage { get; set; }

		public virtual int? AssignedShiftId { get; set; }

		[ForeignKey("AssignedShiftId")]
		public virtual Shift AssignedShift { get; set; }
	}
}