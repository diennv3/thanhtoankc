using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("BankCodes")]
    [Audited]
    public class BankCode : FullAuditedEntity 
    {

		[Required]
		public virtual string Code { get; set; }
		
		[Required]
		public virtual string BankName { get; set; }
		

    }
}