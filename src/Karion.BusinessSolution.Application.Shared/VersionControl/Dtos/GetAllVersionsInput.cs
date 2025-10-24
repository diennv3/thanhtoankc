using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class GetAllVersionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int? MaxVersionFilter { get; set; }
		public int? MinVersionFilter { get; set; }

		public int IsActiveFilter { get; set; }



    }
}