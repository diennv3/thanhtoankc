using Abp.Application.Services.Dto;
using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetAllBankCodesForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string BankNameFilter { get; set; }



    }
}