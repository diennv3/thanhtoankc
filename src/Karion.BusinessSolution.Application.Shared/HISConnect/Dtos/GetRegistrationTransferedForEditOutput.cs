using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.HISConnect.Dtos
{
    public class GetRegistrationTransferedForEditOutput
    {
		public CreateOrEditRegistrationTransferedDto RegistrationTransfered { get; set; }

		public string LichHenKhamMoTaTrieuChung { get; set;}

		public string NguoiBenhUserName { get; set;}


    }
}