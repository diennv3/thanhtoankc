
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.TBHostConfigure.Dtos
{
    public class TechberConfigureDto : EntityDto
    {
		public string Key { get; set; }

		public string Value { get; set; }



    }

    public class GetTenantBankInfoDto
    {
	    public string BankCode { get; set; }
	    public string BankAccount { get; set; }
    }

    public class GetBaoCaoInput
    {
	    public int thang { get; set; }
	    public int nam { get; set; }
    }

    public class BaoCaoDataGroupedByDay
    {
	    public DateTime date { get; set; }
	    public int doanhthu { get; set; }
	    public int soNguoiDaKham { get; set; }
	    public int soNguoiChuaKham { get; set; }
    }
    
    public class GetBaoCaoOutput
    {
	    public int thang { get; set; }
	    public int nam { get; set; }
	    public List<BaoCaoDataGroupedByDay> duLieuTho { get; set; }
    }
}