using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.HanetTenant.Dtos
{
    public class GetHanetTenantDeviceDatasForEditOutput
    {
		public CreateOrEditHanetTenantDeviceDatasDto HanetTenantDeviceDatas { get; set; }

		public string HanetTenantPlaceDatasplaceName { get; set;}


    }
}