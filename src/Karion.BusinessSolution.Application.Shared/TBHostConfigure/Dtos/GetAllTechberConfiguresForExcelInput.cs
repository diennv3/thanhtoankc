using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.TBHostConfigure.Dtos
{
    public class GetAllTechberConfiguresForExcelInput
    {
		public string Filter { get; set; }

		public string KeyFilter { get; set; }

		public string ValueFilter { get; set; }



    }
}