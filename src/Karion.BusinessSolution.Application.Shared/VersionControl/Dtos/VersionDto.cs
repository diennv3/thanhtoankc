
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class VersionDto : EntityDto
    {
		public string Name { get; set; }

		public int Version { get; set; }

		public bool IsActive { get; set; }



    }
}