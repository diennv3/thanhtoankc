
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class CreateOrEditHanetTenantPlaceDatasDto : EntityDto<int?>
    {

		public string placeName { get; set; }
		
		
		public string placeAddress { get; set; }
		
		
		public string placeId { get; set; }
		
		
		public long userId { get; set; }
		
		
		public int tenantId { get; set; }
		
		

    }
}