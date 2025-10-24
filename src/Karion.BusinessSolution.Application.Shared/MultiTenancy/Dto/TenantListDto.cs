using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Karion.BusinessSolution.HanetTenant.Dtos;

namespace Karion.BusinessSolution.MultiTenancy.Dto
{
    public class TenantListDto : EntityDto, IPassivable, IHasCreationTime
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public string EditionDisplayName { get; set; }

        [DisableAuditing]
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public int? EditionId { get; set; }

        public bool IsInTrialPeriod { get; set; }
        
    }

    public class HanetTenantPlaceDatasForListDto : HanetTenantPlaceDatasDto
    {
        public List<HanetTenantDeviceDatasDto> devices { get; set; }
    }
    public class TenantListDtoCustom:TenantListDto
    {
        public List<HanetTenantPlaceDatasForListDto> ListPlace { get; set; }
    }
}