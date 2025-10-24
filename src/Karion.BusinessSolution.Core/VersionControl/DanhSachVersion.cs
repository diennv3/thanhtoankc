using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.VersionControl
{
	[Table("DanhSachVersions")]
    [Audited]
    public class DanhSachVersion : Entity 
    {

		[Required]
		public virtual string Name { get; set; }
		
		public virtual int VersionNumber { get; set; }
		
		public virtual bool IsActive { get; set; }
		

    }
}