
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.NguoiBenhTestNS.Dtos
{
    public class CreateOrEditNguoiBenhTestDto : EntityDto<int?>
    {

		public string Ten { get; set; }
		
		

    }
}