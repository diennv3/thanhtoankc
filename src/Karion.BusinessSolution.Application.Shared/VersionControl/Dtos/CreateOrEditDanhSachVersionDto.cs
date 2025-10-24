
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.VersionControl.Dtos
{
    public class CreateOrEditDanhSachVersionDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public int VersionNumber { get; set; }
		
		
		public bool IsActive { get; set; }
		
		

    }
}