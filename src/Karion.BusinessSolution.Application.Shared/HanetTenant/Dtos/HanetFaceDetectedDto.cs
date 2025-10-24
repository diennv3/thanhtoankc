
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class HanetFaceDetectedDto : EntityDto<long>
    {
		public string placeId { get; set; }

		public string deviceId { get; set; }

		public string userDetectedId { get; set; }

		public string mask { get; set; }

		public string detectImageUrl { get; set; }

		public int flag { get; set; }



    }
}