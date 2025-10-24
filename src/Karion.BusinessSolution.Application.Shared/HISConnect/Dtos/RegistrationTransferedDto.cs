
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.HISConnect.Dtos
{
    public class RegistrationTransferedDto : EntityDto<long>
    {
		public DateTime ApprovedDate { get; set; }

		public DateTime RegistrationDate { get; set; }


		 public int? LichHenKhamId { get; set; }

		 		 public int? NguoiBenhId { get; set; }

		 
    }
}