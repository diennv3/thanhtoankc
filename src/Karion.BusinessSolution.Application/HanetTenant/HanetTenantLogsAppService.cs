

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
	[AbpAuthorize(AppPermissions.Pages_HanetTenantLogs)]
    public class HanetTenantLogsAppService : BusinessSolutionAppServiceBase, IHanetTenantLogsAppService
    {
		 private readonly IRepository<HanetTenantLog> _hanetTenantLogRepository;
		 private readonly IHanetTenantLogsExcelExporter _hanetTenantLogsExcelExporter;
		 

		  public HanetTenantLogsAppService(IRepository<HanetTenantLog> hanetTenantLogRepository, IHanetTenantLogsExcelExporter hanetTenantLogsExcelExporter ) 
		  {
			_hanetTenantLogRepository = hanetTenantLogRepository;
			_hanetTenantLogsExcelExporter = hanetTenantLogsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetHanetTenantLogForViewDto>> GetAll(GetAllHanetTenantLogsInput input)
         {
			
			var filteredHanetTenantLogs = _hanetTenantLogRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var pagedAndFilteredHanetTenantLogs = filteredHanetTenantLogs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var hanetTenantLogs = from o in pagedAndFilteredHanetTenantLogs
                         select new GetHanetTenantLogForViewDto() {
							HanetTenantLog = new HanetTenantLogDto
							{
                                Value = o.Value,
                                Id = o.Id
							}
						};

            var totalCount = await filteredHanetTenantLogs.CountAsync();

            return new PagedResultDto<GetHanetTenantLogForViewDto>(
                totalCount,
                await hanetTenantLogs.ToListAsync()
            );
         }
		 
		 public async Task<GetHanetTenantLogForViewDto> GetHanetTenantLogForView(int id)
         {
            var hanetTenantLog = await _hanetTenantLogRepository.GetAsync(id);

            var output = new GetHanetTenantLogForViewDto { HanetTenantLog = ObjectMapper.Map<HanetTenantLogDto>(hanetTenantLog) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HanetTenantLogs_Edit)]
		 public async Task<GetHanetTenantLogForEditOutput> GetHanetTenantLogForEdit(EntityDto input)
         {
            var hanetTenantLog = await _hanetTenantLogRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHanetTenantLogForEditOutput {HanetTenantLog = ObjectMapper.Map<CreateOrEditHanetTenantLogDto>(hanetTenantLog)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHanetTenantLogDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantLogs_Create)]
		 protected virtual async Task Create(CreateOrEditHanetTenantLogDto input)
         {
            var hanetTenantLog = ObjectMapper.Map<HanetTenantLog>(input);

			

            await _hanetTenantLogRepository.InsertAsync(hanetTenantLog);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantLogs_Edit)]
		 protected virtual async Task Update(CreateOrEditHanetTenantLogDto input)
         {
            var hanetTenantLog = await _hanetTenantLogRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, hanetTenantLog);
         }

		 [AbpAuthorize(AppPermissions.Pages_HanetTenantLogs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _hanetTenantLogRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHanetTenantLogsToExcel(GetAllHanetTenantLogsForExcelInput input)
         {
			
			var filteredHanetTenantLogs = _hanetTenantLogRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var query = (from o in filteredHanetTenantLogs
                         select new GetHanetTenantLogForViewDto() { 
							HanetTenantLog = new HanetTenantLogDto
							{
                                Value = o.Value,
                                Id = o.Id
							}
						 });


            var hanetTenantLogListDtos = await query.ToListAsync();

            return _hanetTenantLogsExcelExporter.ExportToFile(hanetTenantLogListDtos);
         }


    }
}