
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class CreateOrEditHanetTenantDeviceDatasDto : EntityDto<int?>
    {

		public string deviceId { get; set; }
		
		
		public string deviceName { get; set; }
		
		
		public bool deviceStatus { get; set; }
		
		
		public DateTime lastCheck { get; set; }
		
		
		public int tenantId { get; set; }
		
		
		 public int? HanetTenantPlaceDatasId { get; set; }
		 
		 
    }
}