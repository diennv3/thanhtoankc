using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class GetAllHanetTenantLogsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ValueFilter { get; set; }



    }
}