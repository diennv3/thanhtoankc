

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.NguoiBenhTestNS.Exporting;
using Karion.BusinessSolution.NguoiBenhTestNS.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.NguoiBenhTestNS
{
	[AbpAuthorize(AppPermissions.Pages_NguoiBenhTests)]
    public class NguoiBenhTestsAppService : BusinessSolutionAppServiceBase, INguoiBenhTestsAppService
    {
		 private readonly IRepository<NguoiBenhTest> _nguoiBenhTestRepository;
		 private readonly INguoiBenhTestsExcelExporter _nguoiBenhTestsExcelExporter;
		 

		  public NguoiBenhTestsAppService(IRepository<NguoiBenhTest> nguoiBenhTestRepository, INguoiBenhTestsExcelExporter nguoiBenhTestsExcelExporter ) 
		  {
			_nguoiBenhTestRepository = nguoiBenhTestRepository;
			_nguoiBenhTestsExcelExporter = nguoiBenhTestsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetNguoiBenhTestForViewDto>> GetAll(GetAllNguoiBenhTestsInput input)
         {
			
			var filteredNguoiBenhTests = _nguoiBenhTestRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter);

			var pagedAndFilteredNguoiBenhTests = filteredNguoiBenhTests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var nguoiBenhTests = from o in pagedAndFilteredNguoiBenhTests
                         select new GetNguoiBenhTestForViewDto() {
							NguoiBenhTest = new NguoiBenhTestDto
							{
                                Ten = o.Ten,
                                Id = o.Id
							}
						};

            var totalCount = await filteredNguoiBenhTests.CountAsync();

            return new PagedResultDto<GetNguoiBenhTestForViewDto>(
                totalCount,
                await nguoiBenhTests.ToListAsync()
            );
         }
		 
		 public async Task<GetNguoiBenhTestForViewDto> GetNguoiBenhTestForView(int id)
         {
            var nguoiBenhTest = await _nguoiBenhTestRepository.GetAsync(id);

            var output = new GetNguoiBenhTestForViewDto { NguoiBenhTest = ObjectMapper.Map<NguoiBenhTestDto>(nguoiBenhTest) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhTests_Edit)]
		 public async Task<GetNguoiBenhTestForEditOutput> GetNguoiBenhTestForEdit(EntityDto input)
         {
            var nguoiBenhTest = await _nguoiBenhTestRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetNguoiBenhTestForEditOutput {NguoiBenhTest = ObjectMapper.Map<CreateOrEditNguoiBenhTestDto>(nguoiBenhTest)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditNguoiBenhTestDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhTests_Create)]
		 protected virtual async Task Create(CreateOrEditNguoiBenhTestDto input)
         {
            var nguoiBenhTest = ObjectMapper.Map<NguoiBenhTest>(input);

			

            await _nguoiBenhTestRepository.InsertAsync(nguoiBenhTest);
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhTests_Edit)]
		 protected virtual async Task Update(CreateOrEditNguoiBenhTestDto input)
         {
            var nguoiBenhTest = await _nguoiBenhTestRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, nguoiBenhTest);
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhTests_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _nguoiBenhTestRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetNguoiBenhTestsToExcel(GetAllNguoiBenhTestsForExcelInput input)
         {
			
			var filteredNguoiBenhTests = _nguoiBenhTestRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter);

			var query = (from o in filteredNguoiBenhTests
                         select new GetNguoiBenhTestForViewDto() { 
							NguoiBenhTest = new NguoiBenhTestDto
							{
                                Ten = o.Ten,
                                Id = o.Id
							}
						 });


            var nguoiBenhTestListDtos = await query.ToListAsync();

            return _nguoiBenhTestsExcelExporter.ExportToFile(nguoiBenhTestListDtos);
         }


    }
}