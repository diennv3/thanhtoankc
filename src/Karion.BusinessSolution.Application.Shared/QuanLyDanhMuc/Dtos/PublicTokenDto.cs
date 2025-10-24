
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class PublicTokenDto : EntityDto<long>
    {
		public DateTime TimeSet { get; set; }

		public DateTime TimeExpire { get; set; }

		public string Token { get; set; }

		public string PrivateKey { get; set; }

		public string DeviceVerificationCode { get; set; }

		public string LastAccessDeviceVerificationCode { get; set; }

		public bool IsTokenLocked { get; set; }


		 public int? NguoiBenhId { get; set; }

		 
    }
}