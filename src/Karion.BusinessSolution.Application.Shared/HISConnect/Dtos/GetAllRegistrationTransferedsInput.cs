using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.HISConnect.Dtos
{
    public class GetAllRegistrationTransferedsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxApprovedDateFilter { get; set; }
		public DateTime? MinApprovedDateFilter { get; set; }

		public DateTime? MaxRegistrationDateFilter { get; set; }
		public DateTime? MinRegistrationDateFilter { get; set; }


		 public string LichHenKhamMoTaTrieuChungFilter { get; set; }

		 		 public string NguoiBenhUserNameFilter { get; set; }

		 
    }
}