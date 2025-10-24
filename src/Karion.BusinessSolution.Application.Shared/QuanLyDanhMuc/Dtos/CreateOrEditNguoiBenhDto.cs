
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditNguoiBenhDto : EntityDto<int?>
    {

		[Required]
		[StringLength(NguoiBenhConsts.MaxHoVaTenLength, MinimumLength = NguoiBenhConsts.MinHoVaTenLength)]
		public string HoVaTen { get; set; }
		
		
		public int NgaySinh { get; set; }
		
		
		public string GioiTinh { get; set; }
		
		
		public string DiaChi { get; set; }
		
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxUserNameLength, MinimumLength = NguoiBenhConsts.MinUserNameLength)]
		public string UserName { get; set; }
		
		
		public int AccessFailedCount { get; set; }
		
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxPhoneNumberLength, MinimumLength = NguoiBenhConsts.MinPhoneNumberLength)]
		public string PhoneNumber { get; set; }
		
		
		[Required]
		[StringLength(NguoiBenhConsts.MaxEmailAddressLength, MinimumLength = NguoiBenhConsts.MinEmailAddressLength)]
		public string EmailAddress { get; set; }
		
		
		[StringLength(NguoiBenhConsts.MaxEmailConfirmationCodeLength, MinimumLength = NguoiBenhConsts.MinEmailConfirmationCodeLength)]
		public string EmailConfirmationCode { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		public bool IsEmailConfirmed { get; set; }
		
		
		public bool IsPhoneNumberConfirmed { get; set; }
		
		
		[StringLength(NguoiBenhConsts.MaxPasswordResetCodeLength, MinimumLength = NguoiBenhConsts.MinPasswordResetCodeLength)]
		public string PasswordResetCode { get; set; }
		
		
		public string ProfilePicture { get; set; }
		
		public string Password { get; set; }
		
		
		public string Token { get; set; }
		
		
		public DateTime TokenExpire { get; set; }
		
		
		public int ThangSinh { get; set; }
		
		
		public int NamSinh { get; set; }
		
		
		public string SoTheBHYT { get; set; }
		
		
		public string NoiDkBanDau { get; set; }
		
		
		public string MaDonViBHXH { get; set; }
		
		
		public DateTime GiaTriSuDungTuNgay { get; set; }
		
		
		public DateTime ThoiDiemDuNam { get; set; }

		public TimeSpan? WorkShiftStart { get; set; }
		public TimeSpan? WorkShiftEnd { get; set; }
		public string WorkShiftName { get; set; }
		public int? WorkDaysMask { get; set; }
		
		public int? AssignedShiftId { get; set; }
	}
	public class AvatarChangeDto
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Token { get; set; }
		public string JpegFileName { get; set; }
		public byte[] Data { get; set; }
	}
}