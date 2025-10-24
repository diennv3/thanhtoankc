using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.TBHostConfigure.Dtos
{
    public class GetTechberConfigureForEditOutput
    {
		public CreateOrEditTechberConfigureDto TechberConfigure { get; set; }


    }
}