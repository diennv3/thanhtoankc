using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class GetAllDanhSachVersionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int? MaxVersionNumberFilter { get; set; }
		public int? MinVersionNumberFilter { get; set; }

		public int IsActiveFilter { get; set; }



    }
}