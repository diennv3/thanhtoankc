using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class GetAllHanetFaceDetectedsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string placeIdFilter { get; set; }

		public string deviceIdFilter { get; set; }

		public string userDetectedIdFilter { get; set; }

		public string maskFilter { get; set; }

		public string detectImageUrlFilter { get; set; }

		public int? MaxflagFilter { get; set; }
		public int? MinflagFilter { get; set; }



    }
}