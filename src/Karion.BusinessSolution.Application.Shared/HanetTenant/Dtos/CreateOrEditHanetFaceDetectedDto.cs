
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class CreateOrEditHanetFaceDetectedDto : EntityDto<long?>
    {

		public string placeId { get; set; }
		
		
		public string deviceId { get; set; }
		
		
		public string userDetectedId { get; set; }
		
		
		public string mask { get; set; }
		
		
		public string detectImageUrl { get; set; }
		
		
		public int flag { get; set; }
		
		

    }
}