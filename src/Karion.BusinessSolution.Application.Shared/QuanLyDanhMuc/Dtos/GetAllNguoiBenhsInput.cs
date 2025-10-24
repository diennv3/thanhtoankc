using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllNguoiBenhsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string HoVaTenFilter { get; set; }
        public int? MinNgaySinhFilter { get; set; }
        public int? MaxNgaySinhFilter { get; set; }
        public string GioiTinhFilter { get; set; }
        public string DiaChiFilter { get; set; }
        public string UserNameFilter { get; set; }

        public int? MinAccessFailedCountFilter { get; set; }
        public int? MaxAccessFailedCountFilter { get; set; }

        public string PhoneNumberFilter { get; set; }
        public string EmailAddressFilter { get; set; }
        public string EmailConfirmationCodeFilter { get; set; }

        // keep as int with -1 meaning "not filtering"
        public int IsActiveFilter { get; set; } = -1;
        public int IsEmailConfirmedFilter { get; set; } = -1;
        public int IsPhoneNumberConfirmedFilter { get; set; } = -1;

        public string PasswordResetCodeFilter { get; set; }
        public string ProfilePictureFilter { get; set; }
        public string PasswordFilter { get; set; }
        public string TokenFilter { get; set; }

        public DateTime? MinTokenExpireFilter { get; set; }
        public DateTime? MaxTokenExpireFilter { get; set; }

        public int? MinThangSinhFilter { get; set; }
        public int? MaxThangSinhFilter { get; set; }
        public int? MinNamSinhFilter { get; set; }
        public int? MaxNamSinhFilter { get; set; }

        public string SoTheBHYTFilter { get; set; }
        public string NoiDkBanDauFilter { get; set; }
        public string MaDonViBHXHFilter { get; set; }

        public DateTime? MinGiaTriSuDungTuNgayFilter { get; set; }
        public DateTime? MaxGiaTriSuDungTuNgayFilter { get; set; }

        public DateTime? MinThoiDiemDuNamFilter { get; set; }
        public DateTime? MaxThoiDiemDuNamFilter { get; set; }
    }
}