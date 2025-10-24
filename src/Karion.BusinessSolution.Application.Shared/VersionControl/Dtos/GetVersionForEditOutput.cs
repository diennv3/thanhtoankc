using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class GetVersionForEditOutput
    {
		public CreateOrEditVersionDto Version { get; set; }


    }
}