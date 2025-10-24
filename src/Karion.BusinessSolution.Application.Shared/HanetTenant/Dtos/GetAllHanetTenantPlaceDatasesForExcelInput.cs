using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class GetAllHanetTenantPlaceDatasesForExcelInput
    {
		public string Filter { get; set; }

		public string placeNameFilter { get; set; }

		public string placeAddressFilter { get; set; }

		public string placeIdFilter { get; set; }

		public long? MaxuserIdFilter { get; set; }
		public long? MinuserIdFilter { get; set; }

		public int? MaxtenantIdFilter { get; set; }
		public int? MintenantIdFilter { get; set; }



    }
}