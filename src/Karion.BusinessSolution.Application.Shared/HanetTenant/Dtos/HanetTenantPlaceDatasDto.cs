
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class HanetTenantPlaceDatasDto : EntityDto
    {
		public string placeName { get; set; }

		public string placeAddress { get; set; }

		public string placeId { get; set; }

		public long userId { get; set; }

		public int tenantId { get; set; }



    }
}