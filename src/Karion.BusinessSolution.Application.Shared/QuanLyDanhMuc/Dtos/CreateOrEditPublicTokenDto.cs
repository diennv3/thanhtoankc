
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditPublicTokenDto : EntityDto<long?>
    {

		public DateTime TimeSet { get; set; }
		
		
		public DateTime TimeExpire { get; set; }
		
		
		public string Token { get; set; }
		
		
		public string PrivateKey { get; set; }
		
		
		[StringLength(PublicTokenConsts.MaxDeviceVerificationCodeLength, MinimumLength = PublicTokenConsts.MinDeviceVerificationCodeLength)]
		public string DeviceVerificationCode { get; set; }
		
		
		[StringLength(PublicTokenConsts.MaxLastAccessDeviceVerificationCodeLength, MinimumLength = PublicTokenConsts.MinLastAccessDeviceVerificationCodeLength)]
		public string LastAccessDeviceVerificationCode { get; set; }
		
		
		public bool IsTokenLocked { get; set; }
		
		
		 public int? NguoiBenhId { get; set; }
		 
		 
    }
}