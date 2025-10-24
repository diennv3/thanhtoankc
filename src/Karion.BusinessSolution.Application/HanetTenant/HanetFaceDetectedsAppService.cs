

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
	[AbpAuthorize(AppPermissions.Pages_HanetFaceDetecteds)]
    public class HanetFaceDetectedsAppService : BusinessSolutionAppServiceBase, IHanetFaceDetectedsAppService
    {
		 private readonly IRepository<HanetFaceDetected, long> _hanetFaceDetectedRepository;
		 private readonly IHanetFaceDetectedsExcelExporter _hanetFaceDetectedsExcelExporter;
		 

		  public HanetFaceDetectedsAppService(IRepository<HanetFaceDetected, long> hanetFaceDetectedRepository, IHanetFaceDetectedsExcelExporter hanetFaceDetectedsExcelExporter ) 
		  {
			_hanetFaceDetectedRepository = hanetFaceDetectedRepository;
			_hanetFaceDetectedsExcelExporter = hanetFaceDetectedsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetHanetFaceDetectedForViewDto>> GetAll(GetAllHanetFaceDetectedsInput input)
         {
			
			var filteredHanetFaceDetecteds = _hanetFaceDetectedRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.placeId.Contains(input.Filter) || e.deviceId.Contains(input.Filter) || e.userDetectedId.Contains(input.Filter) || e.mask.Contains(input.Filter) || e.detectImageUrl.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeIdFilter),  e => e.placeId == input.placeIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.deviceIdFilter),  e => e.deviceId == input.deviceIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.userDetectedIdFilter),  e => e.userDetectedId == input.userDetectedIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.maskFilter),  e => e.mask == input.maskFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.detectImageUrlFilter),  e => e.detectImageUrl == input.detectImageUrlFilter)
						.WhereIf(input.MinflagFilter != null, e => e.flag >= input.MinflagFilter)
						.WhereIf(input.MaxflagFilter != null, e => e.flag <= input.MaxflagFilter);

			var pagedAndFilteredHanetFaceDetecteds = filteredHanetFaceDetecteds
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var hanetFaceDetecteds = from o in pagedAndFilteredHanetFaceDetecteds
                         select new GetHanetFaceDetectedForViewDto() {
							HanetFaceDetected = new HanetFaceDetectedDto
							{
                                placeId = o.placeId,
                                deviceId = o.deviceId,
                                userDetectedId = o.userDetectedId,
                                mask = o.mask,
                                detectImageUrl = o.detectImageUrl,
                                flag = o.flag,
                                Id = o.Id
							}
						};

            var totalCount = await filteredHanetFaceDetecteds.CountAsync();

            return new PagedResultDto<GetHanetFaceDetectedForViewDto>(
                totalCount,
                await hanetFaceDetecteds.ToListAsync()
            );
         }
		 
		 public async Task<GetHanetFaceDetectedForViewDto> GetHanetFaceDetectedForView(long id)
         {
            var hanetFaceDetected = await _hanetFaceDetectedRepository.GetAsync(id);

            var output = new GetHanetFaceDetectedForViewDto { HanetFaceDetected = ObjectMapper.Map<HanetFaceDetectedDto>(hanetFaceDetected) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HanetFaceDetecteds_Edit)]
		 public async Task<GetHanetFaceDetectedForEditOutput> GetHanetFaceDetectedForEdit(EntityDto<long> input)
         {
            var hanetFaceDetected = await _hanetFaceDetectedRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHanetFaceDetectedForEditOutput {HanetFaceDetected = ObjectMapper.Map<CreateOrEditHanetFaceDetectedDto>(hanetFaceDetected)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHanetFaceDetectedDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetFaceDetecteds_Create)]
		 protected virtual async Task Create(CreateOrEditHanetFaceDetectedDto input)
         {
            var hanetFaceDetected = ObjectMapper.Map<HanetFaceDetected>(input);

			

            await _hanetFaceDetectedRepository.InsertAsync(hanetFaceDetected);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetFaceDetecteds_Edit)]
		 protected virtual async Task Update(CreateOrEditHanetFaceDetectedDto input)
         {
            var hanetFaceDetected = await _hanetFaceDetectedRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, hanetFaceDetected);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetFaceDetecteds_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _hanetFaceDetectedRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHanetFaceDetectedsToExcel(GetAllHanetFaceDetectedsForExcelInput input)
         {
			
			var filteredHanetFaceDetecteds = _hanetFaceDetectedRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.placeId.Contains(input.Filter) || e.deviceId.Contains(input.Filter) || e.userDetectedId.Contains(input.Filter) || e.mask.Contains(input.Filter) || e.detectImageUrl.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.placeIdFilter),  e => e.placeId == input.placeIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.deviceIdFilter),  e => e.deviceId == input.deviceIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.userDetectedIdFilter),  e => e.userDetectedId == input.userDetectedIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.maskFilter),  e => e.mask == input.maskFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.detectImageUrlFilter),  e => e.detectImageUrl == input.detectImageUrlFilter)
						.WhereIf(input.MinflagFilter != null, e => e.flag >= input.MinflagFilter)
						.WhereIf(input.MaxflagFilter != null, e => e.flag <= input.MaxflagFilter);

			var query = (from o in filteredHanetFaceDetecteds
                         select new GetHanetFaceDetectedForViewDto() { 
							HanetFaceDetected = new HanetFaceDetectedDto
							{
                                placeId = o.placeId,
                                deviceId = o.deviceId,
                                userDetectedId = o.userDetectedId,
                                mask = o.mask,
                                detectImageUrl = o.detectImageUrl,
                                flag = o.flag,
                                Id = o.Id
							}
						 });


            var hanetFaceDetectedListDtos = await query.ToListAsync();

            return _hanetFaceDetectedsExcelExporter.ExportToFile(hanetFaceDetectedListDtos);
         }


    }
}