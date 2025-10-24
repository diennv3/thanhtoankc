using Karion.BusinessSolution.HanetTenant;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.HanetTenant
{
	[Table("HanetTenantDeviceDatases")]
    [Audited]
    public class HanetTenantDeviceDatas : Entity 
    {

		public virtual string deviceId { get; set; }
		
		public virtual string deviceName { get; set; }
		
		public virtual bool deviceStatus { get; set; }
		
		public virtual DateTime lastCheck { get; set; }
		
		public virtual int tenantId { get; set; }
		

		public virtual int? HanetTenantPlaceDatasId { get; set; }
		
        [ForeignKey("HanetTenantPlaceDatasId")]
		public HanetTenantPlaceDatas HanetTenantPlaceDatasFk { get; set; }
		
    }
}