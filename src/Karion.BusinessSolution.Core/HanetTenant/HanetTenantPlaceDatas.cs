using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.HanetTenant
{
	[Table("HanetTenantPlaceDatases")]
    [Audited]
    public class HanetTenantPlaceDatas : Entity 
    {

		public virtual string placeName { get; set; }
		
		public virtual string placeAddress { get; set; }
		
		public virtual string placeId { get; set; }
		
		public virtual long userId { get; set; }
		
		public virtual int tenantId { get; set; }
		

    }
}