using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.TBHostConfigure
{
	[Table("TechberConfigures")]
    [Audited]
    public class TechberConfigure : Entity 
    {

		public virtual string Key { get; set; }
		
		public virtual string Value { get; set; }
		

    }
}