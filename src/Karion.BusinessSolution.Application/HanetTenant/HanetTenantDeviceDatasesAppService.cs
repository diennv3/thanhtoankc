using Karion.BusinessSolution.HanetTenant;


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
	[AbpAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases)]
    public class HanetTenantDeviceDatasesAppService : BusinessSolutionAppServiceBase, IHanetTenantDeviceDatasesAppService
    {
		 private readonly IRepository<HanetTenantDeviceDatas> _hanetTenantDeviceDatasRepository;
		 private readonly IHanetTenantDeviceDatasesExcelExporter _hanetTenantDeviceDatasesExcelExporter;
		 private readonly IRepository<HanetTenantPlaceDatas,int> _lookup_hanetTenantPlaceDatasRepository;
		 

		  public HanetTenantDeviceDatasesAppService(IRepository<HanetTenantDeviceDatas> hanetTenantDeviceDatasRepository, IHanetTenantDeviceDatasesExcelExporter hanetTenantDeviceDatasesExcelExporter , IRepository<HanetTenantPlaceDatas, int> lookup_hanetTenantPlaceDatasRepository) 
		  {
			_hanetTenantDeviceDatasRepository = hanetTenantDeviceDatasRepository;
			_hanetTenantDeviceDatasesExcelExporter = hanetTenantDeviceDatasesExcelExporter;
			_lookup_hanetTenantPlaceDatasRepository = lookup_hanetTenantPlaceDatasRepository;
		
		  }

		 public async Task<PagedResultDto<GetHanetTenantDeviceDatasForViewDto>> GetAll(GetAllHanetTenantDeviceDatasesInput input)
         {
			
			var filteredHanetTenantDeviceDatases = _hanetTenantDeviceDatasRepository.GetAll()
						.Include( e => e.HanetTenantPlaceDatasFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.deviceId.Contains(input.Filter) || e.deviceName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.deviceIdFilter),  e => e.deviceId == input.deviceIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.deviceNameFilter),  e => e.deviceName == input.deviceNameFilter)
						.WhereIf(input.deviceStatusFilter > -1,  e => (input.deviceStatusFilter == 1 && e.deviceStatus) || (input.deviceStatusFilter == 0 && !e.deviceStatus) )
						.WhereIf(input.MinlastCheckFilter != null, e => e.lastCheck >= input.MinlastCheckFilter)
						.WhereIf(input.MaxlastCheckFilter != null, e => e.lastCheck <= input.MaxlastCheckFilter)
						.WhereIf(input.MintenantIdFilter != null, e => e.tenantId >= input.MintenantIdFilter)
						.WhereIf(input.MaxtenantIdFilter != null, e => e.tenantId <= input.MaxtenantIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.HanetTenantPlaceDatasplaceNameFilter), e => e.HanetTenantPlaceDatasFk != null && e.HanetTenantPlaceDatasFk.placeName == input.HanetTenantPlaceDatasplaceNameFilter);

			var pagedAndFilteredHanetTenantDeviceDatases = filteredHanetTenantDeviceDatases
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var hanetTenantDeviceDatases = from o in pagedAndFilteredHanetTenantDeviceDatases
                         join o1 in _lookup_hanetTenantPlaceDatasRepository.GetAll() on o.HanetTenantPlaceDatasId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetHanetTenantDeviceDatasForViewDto() {
							HanetTenantDeviceDatas = new HanetTenantDeviceDatasDto
							{
                                deviceId = o.deviceId,
                                deviceName = o.deviceName,
                                deviceStatus = o.deviceStatus,
                                lastCheck = o.lastCheck,
                                tenantId = o.tenantId,
                                Id = o.Id
							},
                         	HanetTenantPlaceDatasplaceName = s1 == null || s1.placeName == null ? "" : s1.placeName.ToString()
						};

            var totalCount = await filteredHanetTenantDeviceDatases.CountAsync();

            return new PagedResultDto<GetHanetTenantDeviceDatasForViewDto>(
                totalCount,
                await hanetTenantDeviceDatases.ToListAsync()
            );
         }
		 
		 public async Task<GetHanetTenantDeviceDatasForViewDto> GetHanetTenantDeviceDatasForView(int id)
         {
            var hanetTenantDeviceDatas = await _hanetTenantDeviceDatasRepository.GetAsync(id);

            var output = new GetHanetTenantDeviceDatasForViewDto { HanetTenantDeviceDatas = ObjectMapper.Map<HanetTenantDeviceDatasDto>(hanetTenantDeviceDatas) };

		    if (output.HanetTenantDeviceDatas.HanetTenantPlaceDatasId != null)
            {
                var _lookupHanetTenantPlaceDatas = await _lookup_hanetTenantPlaceDatasRepository.FirstOrDefaultAsync((int)output.HanetTenantDeviceDatas.HanetTenantPlaceDatasId);
                output.HanetTenantPlaceDatasplaceName = _lookupHanetTenantPlaceDatas?.placeName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases_Edit)]
		 public async Task<GetHanetTenantDeviceDatasForEditOutput> GetHanetTenantDeviceDatasForEdit(EntityDto input)
         {
            var hanetTenantDeviceDatas = await _hanetTenantDeviceDatasRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHanetTenantDeviceDatasForEditOutput {HanetTenantDeviceDatas = ObjectMapper.Map<CreateOrEditHanetTenantDeviceDatasDto>(hanetTenantDeviceDatas)};

		    if (output.HanetTenantDeviceDatas.HanetTenantPlaceDatasId != null)
            {
                var _lookupHanetTenantPlaceDatas = await _lookup_hanetTenantPlaceDatasRepository.FirstOrDefaultAsync((int)output.HanetTenantDeviceDatas.HanetTenantPlaceDatasId);
                output.HanetTenantPlaceDatasplaceName = _lookupHanetTenantPlaceDatas?.placeName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHanetTenantDeviceDatasDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases_Create)]
		 protected virtual async Task Create(CreateOrEditHanetTenantDeviceDatasDto input)
         {
            var hanetTenantDeviceDatas = ObjectMapper.Map<HanetTenantDeviceDatas>(input);

			

            await _hanetTenantDeviceDatasRepository.InsertAsync(hanetTenantDeviceDatas);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases_Edit)]
		 protected virtual async Task Update(CreateOrEditHanetTenantDeviceDatasDto input)
         {
            var hanetTenantDeviceDatas = await _hanetTenantDeviceDatasRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, hanetTenantDeviceDatas);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _hanetTenantDeviceDatasRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHanetTenantDeviceDatasesToExcel(GetAllHanetTenantDeviceDatasesForExcelInput input)
         {
			
			var filteredHanetTenantDeviceDatases = _hanetTenantDeviceDatasRepository.GetAll()
						.Include( e => e.HanetTenantPlaceDatasFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.deviceId.Contains(input.Filter) || e.deviceName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.deviceIdFilter),  e => e.deviceId == input.deviceIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.deviceNameFilter),  e => e.deviceName == input.deviceNameFilter)
						.WhereIf(input.deviceStatusFilter > -1,  e => (input.deviceStatusFilter == 1 && e.deviceStatus) || (input.deviceStatusFilter == 0 && !e.deviceStatus) )
						.WhereIf(input.MinlastCheckFilter != null, e => e.lastCheck >= input.MinlastCheckFilter)
						.WhereIf(input.MaxlastCheckFilter != null, e => e.lastCheck <= input.MaxlastCheckFilter)
						.WhereIf(input.MintenantIdFilter != null, e => e.tenantId >= input.MintenantIdFilter)
						.WhereIf(input.MaxtenantIdFilter != null, e => e.tenantId <= input.MaxtenantIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.HanetTenantPlaceDatasplaceNameFilter), e => e.HanetTenantPlaceDatasFk != null && e.HanetTenantPlaceDatasFk.placeName == input.HanetTenantPlaceDatasplaceNameFilter);

			var query = (from o in filteredHanetTenantDeviceDatases
                         join o1 in _lookup_hanetTenantPlaceDatasRepository.GetAll() on o.HanetTenantPlaceDatasId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetHanetTenantDeviceDatasForViewDto() { 
							HanetTenantDeviceDatas = new HanetTenantDeviceDatasDto
							{
                                deviceId = o.deviceId,
                                deviceName = o.deviceName,
                                deviceStatus = o.deviceStatus,
                                lastCheck = o.lastCheck,
                                tenantId = o.tenantId,
                                Id = o.Id
							},
                         	HanetTenantPlaceDatasplaceName = s1 == null || s1.placeName == null ? "" : s1.placeName.ToString()
						 });


            var hanetTenantDeviceDatasListDtos = await query.ToListAsync();

            return _hanetTenantDeviceDatasesExcelExporter.ExportToFile(hanetTenantDeviceDatasListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_HanetTenantDeviceDatases)]
         public async Task<PagedResultDto<HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableDto>> GetAllHanetTenantPlaceDatasForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_hanetTenantPlaceDatasRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.placeName != null && e.placeName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var hanetTenantPlaceDatasList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableDto>();
			foreach(var hanetTenantPlaceDatas in hanetTenantPlaceDatasList){
				lookupTableDtoList.Add(new HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableDto
				{
					Id = hanetTenantPlaceDatas.Id,
					DisplayName = hanetTenantPlaceDatas.placeName?.ToString()
				});
			}

            return new PagedResultDto<HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}