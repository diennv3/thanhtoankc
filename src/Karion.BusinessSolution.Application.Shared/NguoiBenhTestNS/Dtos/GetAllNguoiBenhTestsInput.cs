using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.NguoiBenhTestNS.Dtos
{
    public class GetAllNguoiBenhTestsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TenFilter { get; set; }



    }
}