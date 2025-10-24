

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.VersionControl.Exporting;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.VersionControl
{
	[AbpAuthorize(AppPermissions.Pages_DanhSachVersions)]
    public class DanhSachVersionsAppService : BusinessSolutionAppServiceBase, IDanhSachVersionsAppService
    {
		 private readonly IRepository<DanhSachVersion> _danhSachVersionRepository;
		 private readonly IDanhSachVersionsExcelExporter _danhSachVersionsExcelExporter;
		 

		  public DanhSachVersionsAppService(IRepository<DanhSachVersion> danhSachVersionRepository, IDanhSachVersionsExcelExporter danhSachVersionsExcelExporter ) 
		  {
			_danhSachVersionRepository = danhSachVersionRepository;
			_danhSachVersionsExcelExporter = danhSachVersionsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetDanhSachVersionForViewDto>> GetAll(GetAllDanhSachVersionsInput input)
         {
			
			var filteredDanhSachVersions = _danhSachVersionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinVersionNumberFilter != null, e => e.VersionNumber >= input.MinVersionNumberFilter)
						.WhereIf(input.MaxVersionNumberFilter != null, e => e.VersionNumber <= input.MaxVersionNumberFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var pagedAndFilteredDanhSachVersions = filteredDanhSachVersions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var danhSachVersions = from o in pagedAndFilteredDanhSachVersions
                         select new GetDanhSachVersionForViewDto() {
							DanhSachVersion = new DanhSachVersionDto
							{
                                Name = o.Name,
                                VersionNumber = o.VersionNumber,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						};

            var totalCount = await filteredDanhSachVersions.CountAsync();

            return new PagedResultDto<GetDanhSachVersionForViewDto>(
                totalCount,
                await danhSachVersions.ToListAsync()
            );
         }
		 
		 public async Task<GetDanhSachVersionForViewDto> GetDanhSachVersionForView(int id)
         {
            var danhSachVersion = await _danhSachVersionRepository.GetAsync(id);

            var output = new GetDanhSachVersionForViewDto { DanhSachVersion = ObjectMapper.Map<DanhSachVersionDto>(danhSachVersion) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DanhSachVersions_Edit)]
		 public async Task<GetDanhSachVersionForEditOutput> GetDanhSachVersionForEdit(EntityDto input)
         {
            var danhSachVersion = await _danhSachVersionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDanhSachVersionForEditOutput {DanhSachVersion = ObjectMapper.Map<CreateOrEditDanhSachVersionDto>(danhSachVersion)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDanhSachVersionDto input)
         {
	         if (input.IsActive)
	         {
		         List<DanhSachVersion> danhSachVersion =
			         await _danhSachVersionRepository.GetAllListAsync(p => p.IsActive && p.Name.Equals(input.Name));
		         foreach (var VARIABLE in danhSachVersion)
		         {
			         VARIABLE.IsActive = false;
			         await _danhSachVersionRepository.UpdateAsync(VARIABLE);
		         }
		         
	         }
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DanhSachVersions_Create)]
		 protected virtual async Task Create(CreateOrEditDanhSachVersionDto input)
         {
            var danhSachVersion = ObjectMapper.Map<DanhSachVersion>(input);

			

            await _danhSachVersionRepository.InsertAsync(danhSachVersion);
         }

		 [AbpAuthorize(AppPermissions.Pages_DanhSachVersions_Edit)]
		 protected virtual async Task Update(CreateOrEditDanhSachVersionDto input)
         {
            var danhSachVersion = await _danhSachVersionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, danhSachVersion);
         }

		 [AbpAuthorize(AppPermissions.Pages_DanhSachVersions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _danhSachVersionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDanhSachVersionsToExcel(GetAllDanhSachVersionsForExcelInput input)
         {
			
			var filteredDanhSachVersions = _danhSachVersionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinVersionNumberFilter != null, e => e.VersionNumber >= input.MinVersionNumberFilter)
						.WhereIf(input.MaxVersionNumberFilter != null, e => e.VersionNumber <= input.MaxVersionNumberFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var query = (from o in filteredDanhSachVersions
                         select new GetDanhSachVersionForViewDto() { 
							DanhSachVersion = new DanhSachVersionDto
							{
                                Name = o.Name,
                                VersionNumber = o.VersionNumber,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						 });


            var danhSachVersionListDtos = await query.ToListAsync();

            return _danhSachVersionsExcelExporter.ExportToFile(danhSachVersionListDtos);
         }


    }
}