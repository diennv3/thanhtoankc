using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.VersionControl
{
	[Table("Versions")]
    [Audited]
    public class Version : Entity 
    {

		[Required]
		public virtual string Name { get; set; }
		
		public virtual int Versions { get; set; }
		
		public virtual bool IsActive { get; set; }
		

    }
}