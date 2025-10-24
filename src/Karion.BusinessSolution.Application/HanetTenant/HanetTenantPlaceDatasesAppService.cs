

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.HanetTenant.Exporting;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.HanetTenant
{
	[AbpAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases)]
    public class HanetTenantPlaceDatasesAppService : BusinessSolutionAppServiceBase, IHanetTenantPlaceDatasesAppService
    {
		 private readonly IRepository<HanetTenantPlaceDatas> _hanetTenantPlaceDatasRepository;
		 private readonly IHanetTenantPlaceDatasesExcelExporter _hanetTenantPlaceDatasesExcelExporter;
		 private readonly HanetAppservices.HanetAppservices.HanetAppservices _hanetAppservices;

		  public HanetTenantPlaceDatasesAppService(IRepository<HanetTenantPlaceDatas> hanetTenantPlaceDatasRepository, IHanetTenantPlaceDatasesExcelExporter hanetTenantPlaceDatasesExcelExporter,
			  HanetAppservices.HanetAppservices.HanetAppservices hanetAppservices
			  ) 
		  {
			_hanetTenantPlaceDatasRepository = hanetTenantPlaceDatasRepository;
			_hanetTenantPlaceDatasesExcelExporter = hanetTenantPlaceDatasesExcelExporter;
			_hanetAppservices = hanetAppservices;
		  }

		 public async Task<PagedResultDto<GetHanetTenantPlaceDatasForViewDto>> GetAll(GetAllHanetTenantPlaceDatasesInput input)
         {
			
			var filteredHanetTenantPlaceDatases = _hanetTenantPlaceDatasRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.placeName.Contains(input.Filter) || e.placeAddress.Contains(input.Filter) || e.placeId.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeNameFilter),  e => e.placeName == input.placeNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeAddressFilter),  e => e.placeAddress == input.placeAddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeIdFilter),  e => e.placeId == input.placeIdFilter)
						.WhereIf(input.MinuserIdFilter != null, e => e.userId >= input.MinuserIdFilter)
						.WhereIf(input.MaxuserIdFilter != null, e => e.userId <= input.MaxuserIdFilter)
						.WhereIf(input.MintenantIdFilter != null, e => e.tenantId >= input.MintenantIdFilter)
						.WhereIf(input.MaxtenantIdFilter != null, e => e.tenantId <= input.MaxtenantIdFilter);

			var pagedAndFilteredHanetTenantPlaceDatases = filteredHanetTenantPlaceDatases
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var hanetTenantPlaceDatases = from o in pagedAndFilteredHanetTenantPlaceDatases
                         select new GetHanetTenantPlaceDatasForViewDto() {
							HanetTenantPlaceDatas = new HanetTenantPlaceDatasDto
							{
                                placeName = o.placeName,
                                placeAddress = o.placeAddress,
                                placeId = o.placeId,
                                userId = o.userId,
                                tenantId = o.tenantId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredHanetTenantPlaceDatases.CountAsync();

            return new PagedResultDto<GetHanetTenantPlaceDatasForViewDto>(
                totalCount,
                await hanetTenantPlaceDatases.ToListAsync()
            );
         }
		 
		 public async Task<GetHanetTenantPlaceDatasForViewDto> GetHanetTenantPlaceDatasForView(int id)
         {
            var hanetTenantPlaceDatas = await _hanetTenantPlaceDatasRepository.GetAsync(id);

            var output = new GetHanetTenantPlaceDatasForViewDto { HanetTenantPlaceDatas = ObjectMapper.Map<HanetTenantPlaceDatasDto>(hanetTenantPlaceDatas) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases_Edit)]
		 public async Task<GetHanetTenantPlaceDatasForEditOutput> GetHanetTenantPlaceDatasForEdit(EntityDto input)
         {
            var hanetTenantPlaceDatas = await _hanetTenantPlaceDatasRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHanetTenantPlaceDatasForEditOutput {HanetTenantPlaceDatas = ObjectMapper.Map<CreateOrEditHanetTenantPlaceDatasDto>(hanetTenantPlaceDatas)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHanetTenantPlaceDatasDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases_Create)]
		 protected virtual async Task Create(CreateOrEditHanetTenantPlaceDatasDto input)
         {
            var hanetTenantPlaceDatas = new HanetTenantPlaceDatasDto()
            {
	            placeAddress = input.placeAddress,
	            placeId = input.placeId,
	            placeName = input.placeName,
	            tenantId = input.tenantId,
	            userId = input.userId
            };
            await _hanetAppservices.HanetWebhookAddPlace(
	            hanetTenantPlaceDatas);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases_Edit)]
		 protected virtual async Task Update(CreateOrEditHanetTenantPlaceDatasDto input)
         {
            var hanetTenantPlaceDatas = await _hanetTenantPlaceDatasRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, hanetTenantPlaceDatas);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantPlaceDatases_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _hanetTenantPlaceDatasRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHanetTenantPlaceDatasesToExcel(GetAllHanetTenantPlaceDatasesForExcelInput input)
         {
			
			var filteredHanetTenantPlaceDatases = _hanetTenantPlaceDatasRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.placeName.Contains(input.Filter) || e.placeAddress.Contains(input.Filter) || e.placeId.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeNameFilter),  e => e.placeName == input.placeNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeAddressFilter),  e => e.placeAddress == input.placeAddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeIdFilter),  e => e.placeId == input.placeIdFilter)
						.WhereIf(input.MinuserIdFilter != null, e => e.userId >= input.MinuserIdFilter)
						.WhereIf(input.MaxuserIdFilter != null, e => e.userId <= input.MaxuserIdFilter)
						.WhereIf(input.MintenantIdFilter != null, e => e.tenantId >= input.MintenantIdFilter)
						.WhereIf(input.MaxtenantIdFilter != null, e => e.tenantId <= input.MaxtenantIdFilter);

			var query = (from o in filteredHanetTenantPlaceDatases
                         select new GetHanetTenantPlaceDatasForViewDto() { 
							HanetTenantPlaceDatas = new HanetTenantPlaceDatasDto
							{
                                placeName = o.placeName,
                                placeAddress = o.placeAddress,
                                placeId = o.placeId,
                                userId = o.userId,
                                tenantId = o.tenantId,
                                Id = o.Id
							}
						 });


            var hanetTenantPlaceDatasListDtos = await query.ToListAsync();

            return _hanetTenantPlaceDatasesExcelExporter.ExportToFile(hanetTenantPlaceDatasListDtos);
         }


    }
}