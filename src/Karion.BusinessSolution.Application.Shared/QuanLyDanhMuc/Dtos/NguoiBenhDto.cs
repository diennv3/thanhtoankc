
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class NguoiBenhDto : EntityDto
    {
		public string HoVaTen { get; set; }

		public int NgaySinh { get; set; }

		public string GioiTinh { get; set; }

		public string DiaChi { get; set; }

		public string UserName { get; set; }

		public int AccessFailedCount { get; set; }

		public string PhoneNumber { get; set; }

		public string EmailAddress { get; set; }

		public string EmailConfirmationCode { get; set; }

		public bool IsActive { get; set; }

		public bool IsEmailConfirmed { get; set; }

		public bool IsPhoneNumberConfirmed { get; set; }

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
		
		public int? AssignedShiftId { get; set; }
		public string AssignedShiftName { get; set; }
		public string AssignedShiftStart { get; set; } // e.g. "08:00:00"
		public string AssignedShiftEnd { get; set; }   // e.g. "17:00:00"
		public int? AssignedShiftWorkDaysMask { get; set; }
	}
}