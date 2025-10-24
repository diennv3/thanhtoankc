using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.NguoiBenhTestNS
{
	[Table("NguoiBenhTests")]
    [Audited]
    public class NguoiBenhTest : FullAuditedEntity 
    {

		public virtual string Ten { get; set; }
		

    }
}