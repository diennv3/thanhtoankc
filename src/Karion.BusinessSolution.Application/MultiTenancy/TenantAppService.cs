using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Security;
using Microsoft.EntityFrameworkCore;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.Editions.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.MultiTenancy.Dto;
using Karion.BusinessSolution.Url;

namespace Karion.BusinessSolution.MultiTenancy
{
    [AbpAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantAppService : BusinessSolutionAppServiceBase, ITenantAppService
    {
        public IAppUrlService AppUrlService { get; set; }
        public IEventBus EventBus { get; set; }
        private readonly IRepository<HanetTenantPlaceDatas> _hanetTenantRepository;
        private readonly IRepository<HanetTenantDeviceDatas> _hanetDeviceRepository;

        public TenantAppService(IRepository<HanetTenantPlaceDatas> hanetTenantRepository,IRepository<HanetTenantDeviceDatas> hanetDeviceRepository)
        {
            AppUrlService = NullAppUrlService.Instance;
            EventBus = NullEventBus.Instance;
            _hanetTenantRepository = hanetTenantRepository;
            _hanetDeviceRepository = hanetDeviceRepository;
        }

        public async Task<PagedResultDto<TenantListDtoCustom>> GetTenants(GetTenantsInput input)
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition)
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), t => t.Name.Contains(input.Filter) || t.TenancyName.Contains(input.Filter))
                .WhereIf(input.CreationDateStart.HasValue, t => t.CreationTime >= input.CreationDateStart.Value)
                .WhereIf(input.CreationDateEnd.HasValue, t => t.CreationTime <= input.CreationDateEnd.Value)
                .WhereIf(input.SubscriptionEndDateStart.HasValue, t => t.SubscriptionEndDateUtc >= input.SubscriptionEndDateStart.Value.ToUniversalTime())
                .WhereIf(input.SubscriptionEndDateEnd.HasValue, t => t.SubscriptionEndDateUtc <= input.SubscriptionEndDateEnd.Value.ToUniversalTime())
                .WhereIf(input.EditionIdSpecified, t => t.EditionId == input.EditionId);

            var tenantCount = await query.CountAsync();
            var tenants = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            List<TenantListDtoCustom> tenant = new List<TenantListDtoCustom>();

            foreach (var Variable in tenants)
            {
                var data = await _hanetTenantRepository.GetAllListAsync(p=>p.tenantId.Equals(Variable.Id));
                var dataFinally = new List<HanetTenantPlaceDatasForListDto>();
                foreach (var dataDevice in data)
                {
                    
                    dataFinally.Add(new HanetTenantPlaceDatasForListDto()
                    {
                        Id = dataDevice.Id,
                        placeAddress = dataDevice.placeAddress,
                        placeId = dataDevice.placeId,
                        placeName = dataDevice.placeName,
                        tenantId = dataDevice.tenantId,
                        userId = dataDevice.userId,
                        devices = ObjectMapper.Map<List<HanetTenantDeviceDatasDto>>(await _hanetDeviceRepository.GetAllListAsync(p=>p.HanetTenantPlaceDatasId.Equals(dataDevice.Id)))
                    });
                }
                tenant.Add(new TenantListDtoCustom()
                {
                    Id = Variable.Id,
                    Name = Variable.Name,
                    ConnectionString = Variable.ConnectionString,
                    CreationTime = Variable.CreationTime,
                    EditionId = Variable.EditionId,
                    IsActive = Variable.IsActive,
                    TenancyName = Variable.TenancyName,
                    IsInTrialPeriod = Variable.IsInTrialPeriod,
                    EditionDisplayName = Variable.Edition.DisplayName,
                    SubscriptionEndDateUtc = Variable.SubscriptionEndDateUtc, 
                    ListPlace = dataFinally
                });
                
            }
            return new PagedResultDto<TenantListDtoCustom>(
                tenantCount,
                tenant
                );
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Create)]
        [UnitOfWork(IsDisabled = true)]
        public async Task CreateTenant(CreateTenantInput input)
        {
            await TenantManager.CreateWithAdminUserAsync(input.TenancyName,
                input.Name,
                input.AdminPassword,
                input.AdminEmailAddress,
                input.ConnectionString,
                input.IsActive,
                input.EditionId,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail,
                input.SubscriptionEndDateUtc?.ToUniversalTime(),
                input.IsInTrialPeriod,
                AppUrlService.CreateEmailActivationUrlFormat(input.TenancyName)
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task<TenantEditDto> GetTenantForEdit(EntityDto input)
        {
            var tenantEditDto = ObjectMapper.Map<TenantEditDto>(await TenantManager.GetByIdAsync(input.Id));
            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);
            return tenantEditDto;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task UpdateTenant(TenantEditDto input)
        {
            await TenantManager.CheckEditionAsync(input.EditionId, input.IsInTrialPeriod);

            input.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            var tenant = await TenantManager.GetByIdAsync(input.Id);

            if (tenant.EditionId != input.EditionId)
            {
                EventBus.Trigger(new TenantEditionChangedEventData
                {
                    TenantId = input.Id,
                    OldEditionId = tenant.EditionId,
                    NewEditionId = input.EditionId
                });
            }

            ObjectMapper.Map(input, tenant);
            tenant.SubscriptionEndDateUtc = tenant.SubscriptionEndDateUtc?.ToUniversalTime();

            await TenantManager.UpdateAsync(tenant);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Delete)]
        public async Task DeleteTenant(EntityDto input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            await TenantManager.DeleteAsync(tenant);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));
            var featureValues = await TenantManager.GetFeatureValuesAsync(input.Id);

            return new GetTenantFeaturesEditOutput
            {
                Features = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await TenantManager.SetFeatureValuesAsync(input.Id, input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await TenantManager.ResetAllFeaturesAsync(input.Id);
        }

        public async Task UnlockTenantAdmin(EntityDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.Id))
            {
                var tenantAdmin = await UserManager.GetAdminAsync();
                if (tenantAdmin != null)
                {
                    tenantAdmin.Unlock();
                }
            }
        }

      
    }
}