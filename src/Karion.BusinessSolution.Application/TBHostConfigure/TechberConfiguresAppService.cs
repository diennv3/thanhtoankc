

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.TBHostConfigure.Exporting;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.TBHostConfigure
{
	[AbpAuthorize(AppPermissions.Pages_TechberConfigures)]
    public class TechberConfiguresAppService : BusinessSolutionAppServiceBase, ITechberConfiguresAppService
    {
		 private readonly IRepository<TechberConfigure> _techberConfigureRepository;
		 private readonly ITechberConfiguresExcelExporter _techberConfiguresExcelExporter;
		 

		  public TechberConfiguresAppService(IRepository<TechberConfigure> techberConfigureRepository, ITechberConfiguresExcelExporter techberConfiguresExcelExporter ) 
		  {
			_techberConfigureRepository = techberConfigureRepository;
			_techberConfiguresExcelExporter = techberConfiguresExcelExporter;
			
		  }

		  public async Task<string> GetAuthorizationData()
		  {
			  var hanetClientID =
				  await _techberConfigureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_CLIENT_ID");
			  if (hanetClientID.isNull())
			  {
				  return "hanetClientID configuration is missing [[KEY: HANET_GET_CLIENT_ID]]";
			  }
			  return  "https://oauth.hanet.com/oauth2/authorize?response_type=code&client_id="+hanetClientID.Value+"&redirect_uri=https://localhost:8888/api/services/app/HanetAppservices/HanetWebhookGetAuthorizationCode&scope=full";
		  }

		 public async Task<PagedResultDto<GetTechberConfigureForViewDto>> GetAll(GetAllTechberConfiguresInput input)
         {
			
			var filteredTechberConfigures = _techberConfigureRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Key.Contains(input.Filter) || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyFilter),  e => e.Key == input.KeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var pagedAndFilteredTechberConfigures = filteredTechberConfigures
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var techberConfigures = from o in pagedAndFilteredTechberConfigures
                         select new GetTechberConfigureForViewDto() {
							TechberConfigure = new TechberConfigureDto
							{
                                Key = o.Key,
                                Value = o.Value,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTechberConfigures.CountAsync();

            return new PagedResultDto<GetTechberConfigureForViewDto>(
                totalCount,
                await techberConfigures.ToListAsync()
            );
         }
		 
		 public async Task<GetTechberConfigureForViewDto> GetTechberConfigureForView(int id)
         {
            var techberConfigure = await _techberConfigureRepository.GetAsync(id);

            var output = new GetTechberConfigureForViewDto { TechberConfigure = ObjectMapper.Map<TechberConfigureDto>(techberConfigure) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TechberConfigures_Edit)]
		 public async Task<GetTechberConfigureForEditOutput> GetTechberConfigureForEdit(EntityDto input)
         {
            var techberConfigure = await _techberConfigureRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTechberConfigureForEditOutput {TechberConfigure = ObjectMapper.Map<CreateOrEditTechberConfigureDto>(techberConfigure)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTechberConfigureDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TechberConfigures_Create)]
		 protected virtual async Task Create(CreateOrEditTechberConfigureDto input)
         {
            var techberConfigure = ObjectMapper.Map<TechberConfigure>(input);

			

            await _techberConfigureRepository.InsertAsync(techberConfigure);
         }

		 [AbpAuthorize(AppPermissions.Pages_TechberConfigures_Edit)]
		 protected virtual async Task Update(CreateOrEditTechberConfigureDto input)
         {
            var techberConfigure = await _techberConfigureRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, techberConfigure);
         }

		 [AbpAuthorize(AppPermissions.Pages_TechberConfigures_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _techberConfigureRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTechberConfiguresToExcel(GetAllTechberConfiguresForExcelInput input)
         {
			
			var filteredTechberConfigures = _techberConfigureRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Key.Contains(input.Filter) || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyFilter),  e => e.Key == input.KeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var query = (from o in filteredTechberConfigures
                         select new GetTechberConfigureForViewDto() { 
							TechberConfigure = new TechberConfigureDto
							{
                                Key = o.Key,
                                Value = o.Value,
                                Id = o.Id
							}
						 });


            var techberConfigureListDtos = await query.ToListAsync();

            return _techberConfiguresExcelExporter.ExportToFile(techberConfigureListDtos);
         }


    }
}