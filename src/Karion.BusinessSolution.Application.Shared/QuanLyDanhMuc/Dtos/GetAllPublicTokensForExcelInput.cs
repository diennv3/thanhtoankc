using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllPublicTokensForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxTimeSetFilter { get; set; }
		public DateTime? MinTimeSetFilter { get; set; }

		public DateTime? MaxTimeExpireFilter { get; set; }
		public DateTime? MinTimeExpireFilter { get; set; }

		public string TokenFilter { get; set; }

		public string PrivateKeyFilter { get; set; }

		public string DeviceVerificationCodeFilter { get; set; }

		public string LastAccessDeviceVerificationCodeFilter { get; set; }

		public int IsTokenLockedFilter { get; set; }


		 public string NguoiBenhUserNameFilter { get; set; }

		 
    }
}