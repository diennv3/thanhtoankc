
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class CreateOrEditHanetTenantLogDto : EntityDto<int?>
    {

		public string Value { get; set; }
		
		

    }
}