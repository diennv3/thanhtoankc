using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllNguoiBenhsForExcelInput
    {
		public string Filter { get; set; }

		public string HoVaTenFilter { get; set; }

		public string TuoiFilter { get; set; }

		public string GioiTinhFilter { get; set; }

		public string DiaChiFilter { get; set; }

		public string UserNameFilter { get; set; }

		public int? MaxAccessFailedCountFilter { get; set; }
		public int? MinAccessFailedCountFilter { get; set; }

		public string PhoneNumberFilter { get; set; }

		public string EmailAddressFilter { get; set; }

		public string EmailConfirmationCodeFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public int IsEmailConfirmedFilter { get; set; }

		public int IsPhoneNumberConfirmedFilter { get; set; }

		public string PasswordResetCodeFilter { get; set; }

		public string ProfilePictureFilter { get; set; }

		public string PasswordFilter { get; set; }

		public string TokenFilter { get; set; }

		public DateTime? MaxTokenExpireFilter { get; set; }
		public DateTime? MinTokenExpireFilter { get; set; }



    }
}