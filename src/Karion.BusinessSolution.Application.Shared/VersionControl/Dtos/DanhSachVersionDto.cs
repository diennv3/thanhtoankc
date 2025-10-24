
using System;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class DanhSachVersionDto : EntityDto
    {
		public string Name { get; set; }

		public int VersionNumber { get; set; }

		public bool IsActive { get; set; }



    }
}