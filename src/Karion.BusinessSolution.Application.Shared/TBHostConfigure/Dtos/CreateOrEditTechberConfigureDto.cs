
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.TBHostConfigure.Dtos
{
    public class CreateOrEditTechberConfigureDto : EntityDto<int?>
    {

		public string Key { get; set; }
		
		
		public string Value { get; set; }
		
		

    }
}