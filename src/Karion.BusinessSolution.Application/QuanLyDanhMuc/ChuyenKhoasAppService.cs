

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDanhMuc.Exporting;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_ChuyenKhoas)]
    public class ChuyenKhoasAppService : BusinessSolutionAppServiceBase, IChuyenKhoasAppService
    {
		 private readonly IRepository<ChuyenKhoa> _chuyenKhoaRepository;
		 private readonly IChuyenKhoasExcelExporter _chuyenKhoasExcelExporter;
		 

		  public ChuyenKhoasAppService(IRepository<ChuyenKhoa> chuyenKhoaRepository, IChuyenKhoasExcelExporter chuyenKhoasExcelExporter ) 
		  {
			_chuyenKhoaRepository = chuyenKhoaRepository;
			_chuyenKhoasExcelExporter = chuyenKhoasExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetChuyenKhoaForViewDto>> GetAll(GetAllChuyenKhoasInput input)
         {
			
			var filteredChuyenKhoas = _chuyenKhoaRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter),  e => e.MoTa == input.MoTaFilter);

			var pagedAndFilteredChuyenKhoas = filteredChuyenKhoas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chuyenKhoas = from o in pagedAndFilteredChuyenKhoas
                         select new GetChuyenKhoaForViewDto() {
							ChuyenKhoa = new ChuyenKhoaDto
							{
                                Ten = o.Ten,
                                MoTa = o.MoTa,
                                Id = o.Id
							}
						};

            var totalCount = await filteredChuyenKhoas.CountAsync();

            return new PagedResultDto<GetChuyenKhoaForViewDto>(
                totalCount,
                await chuyenKhoas.ToListAsync()
            );
         }
		 
		 public async Task<GetChuyenKhoaForViewDto> GetChuyenKhoaForView(int id)
         {
            var chuyenKhoa = await _chuyenKhoaRepository.GetAsync(id);

            var output = new GetChuyenKhoaForViewDto { ChuyenKhoa = ObjectMapper.Map<ChuyenKhoaDto>(chuyenKhoa) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ChuyenKhoas_Edit)]
		 public async Task<GetChuyenKhoaForEditOutput> GetChuyenKhoaForEdit(EntityDto input)
         {
            var chuyenKhoa = await _chuyenKhoaRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChuyenKhoaForEditOutput {ChuyenKhoa = ObjectMapper.Map<CreateOrEditChuyenKhoaDto>(chuyenKhoa)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChuyenKhoaDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ChuyenKhoas_Create)]
		 protected virtual async Task Create(CreateOrEditChuyenKhoaDto input)
         {
            var chuyenKhoa = ObjectMapper.Map<ChuyenKhoa>(input);

			
			if (AbpSession.TenantId != null)
			{
				chuyenKhoa.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _chuyenKhoaRepository.InsertAsync(chuyenKhoa);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChuyenKhoas_Edit)]
		 protected virtual async Task Update(CreateOrEditChuyenKhoaDto input)
         {
            var chuyenKhoa = await _chuyenKhoaRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, chuyenKhoa);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChuyenKhoas_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _chuyenKhoaRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetChuyenKhoasToExcel(GetAllChuyenKhoasForExcelInput input)
         {
			
			var filteredChuyenKhoas = _chuyenKhoaRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter),  e => e.MoTa == input.MoTaFilter);

			var query = (from o in filteredChuyenKhoas
                         select new GetChuyenKhoaForViewDto() { 
							ChuyenKhoa = new ChuyenKhoaDto
							{
                                Ten = o.Ten,
                                MoTa = o.MoTa,
                                Id = o.Id
							}
						 });


            var chuyenKhoaListDtos = await query.ToListAsync();

            return _chuyenKhoasExcelExporter.ExportToFile(chuyenKhoaListDtos);
         }
		 

    }
}