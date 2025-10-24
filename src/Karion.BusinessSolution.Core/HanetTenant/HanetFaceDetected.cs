using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.HanetTenant
{
	[Table("HanetFaceDetecteds")]
    [Audited]
    public class HanetFaceDetected : FullAuditedEntity<long> 
    {

		public virtual string placeId { get; set; }
		
		public virtual string deviceId { get; set; }
		
		public virtual string userDetectedId { get; set; }
		
		public virtual string mask { get; set; }
		
		public virtual string detectImageUrl { get; set; }
		
		public virtual int flag { get; set; }
		

    }
}