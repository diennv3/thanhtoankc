
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class HanetTenantDeviceDatasDto : EntityDto
    {
		public string deviceId { get; set; }

		public string deviceName { get; set; }

		public bool deviceStatus { get; set; }

		public DateTime lastCheck { get; set; }

		public int tenantId { get; set; }


		 public int? HanetTenantPlaceDatasId { get; set; }

		 
    }
}