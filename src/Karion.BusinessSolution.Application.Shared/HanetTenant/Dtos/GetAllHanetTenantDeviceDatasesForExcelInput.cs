using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class GetAllHanetTenantDeviceDatasesForExcelInput
    {
		public string Filter { get; set; }

		public string deviceIdFilter { get; set; }

		public string deviceNameFilter { get; set; }

		public int deviceStatusFilter { get; set; }

		public DateTime? MaxlastCheckFilter { get; set; }
		public DateTime? MinlastCheckFilter { get; set; }

		public int? MaxtenantIdFilter { get; set; }
		public int? MintenantIdFilter { get; set; }


		 public string HanetTenantPlaceDatasplaceNameFilter { get; set; }

		 
    }
}