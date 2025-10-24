using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Karion.BusinessSolution.HanetTenant
{
	[Table("HanetTenantLogs")]
    public class HanetTenantLog : FullAuditedEntity 
    {

		public virtual string Value { get; set; }
		

    }
}