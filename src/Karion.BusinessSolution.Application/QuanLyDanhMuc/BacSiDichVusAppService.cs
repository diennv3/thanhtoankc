using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.QuanLyDanhMuc;


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
	[AbpAuthorize(AppPermissions.Pages_BacSiDichVus)]
    public class BacSiDichVusAppService : BusinessSolutionAppServiceBase, IBacSiDichVusAppService
    {
		 private readonly IRepository<BacSiDichVu> _bacSiDichVuRepository;
		 private readonly IBacSiDichVusExcelExporter _bacSiDichVusExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<DichVu,int> _lookup_dichVuRepository;
		 

		  public BacSiDichVusAppService(IRepository<BacSiDichVu> bacSiDichVuRepository, IBacSiDichVusExcelExporter bacSiDichVusExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<DichVu, int> lookup_dichVuRepository) 
		  {
			_bacSiDichVuRepository = bacSiDichVuRepository;
			_bacSiDichVusExcelExporter = bacSiDichVusExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_dichVuRepository = lookup_dichVuRepository;
		
		  }

		 public async Task<PagedResultDto<GetBacSiDichVuForViewDto>> GetAll(GetAllBacSiDichVusInput input)
         {
			
			var filteredBacSiDichVus = _bacSiDichVuRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DichVuFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DichVuTenFilter), e => e.DichVuFk != null && e.DichVuFk.Ten == input.DichVuTenFilter);

			var pagedAndFilteredBacSiDichVus = filteredBacSiDichVus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bacSiDichVus = from o in pagedAndFilteredBacSiDichVus
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_dichVuRepository.GetAll() on o.DichVuId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBacSiDichVuForViewDto() {
							BacSiDichVu = new BacSiDichVuDto
							{
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	DichVuTen = s2 == null || s2.Ten == null ? "" : s2.Ten.ToString()
						};

            var totalCount = await filteredBacSiDichVus.CountAsync();

            return new PagedResultDto<GetBacSiDichVuForViewDto>(
                totalCount,
                await bacSiDichVus.ToListAsync()
            );
         }
		 
		 public async Task<GetBacSiDichVuForViewDto> GetBacSiDichVuForView(int id)
         {
            var bacSiDichVu = await _bacSiDichVuRepository.GetAsync(id);

            var output = new GetBacSiDichVuForViewDto { BacSiDichVu = ObjectMapper.Map<BacSiDichVuDto>(bacSiDichVu) };

		    if (output.BacSiDichVu.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BacSiDichVu.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.BacSiDichVu.DichVuId != null)
            {
                var _lookupDichVu = await _lookup_dichVuRepository.FirstOrDefaultAsync((int)output.BacSiDichVu.DichVuId);
                output.DichVuTen = _lookupDichVu?.Ten?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BacSiDichVus_Edit)]
		 public async Task<GetBacSiDichVuForEditOutput> GetBacSiDichVuForEdit(EntityDto input)
         {
            var bacSiDichVu = await _bacSiDichVuRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBacSiDichVuForEditOutput {BacSiDichVu = ObjectMapper.Map<CreateOrEditBacSiDichVuDto>(bacSiDichVu)};

		    if (output.BacSiDichVu.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BacSiDichVu.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.BacSiDichVu.DichVuId != null)
            {
                var _lookupDichVu = await _lookup_dichVuRepository.FirstOrDefaultAsync((int)output.BacSiDichVu.DichVuId);
                output.DichVuTen = _lookupDichVu?.Ten?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBacSiDichVuDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BacSiDichVus_Create)]
		 protected virtual async Task Create(CreateOrEditBacSiDichVuDto input)
         {
            var bacSiDichVu = ObjectMapper.Map<BacSiDichVu>(input);

			
			if (AbpSession.TenantId != null)
			{
				bacSiDichVu.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bacSiDichVuRepository.InsertAsync(bacSiDichVu);
         }

		 [AbpAuthorize(AppPermissions.Pages_BacSiDichVus_Edit)]
		 protected virtual async Task Update(CreateOrEditBacSiDichVuDto input)
         {
            var bacSiDichVu = await _bacSiDichVuRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, bacSiDichVu);
         }

		 [AbpAuthorize(AppPermissions.Pages_BacSiDichVus_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bacSiDichVuRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBacSiDichVusToExcel(GetAllBacSiDichVusForExcelInput input)
         {
			
			var filteredBacSiDichVus = _bacSiDichVuRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DichVuFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DichVuTenFilter), e => e.DichVuFk != null && e.DichVuFk.Ten == input.DichVuTenFilter);

			var query = (from o in filteredBacSiDichVus
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_dichVuRepository.GetAll() on o.DichVuId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBacSiDichVuForViewDto() { 
							BacSiDichVu = new BacSiDichVuDto
							{
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	DichVuTen = s2 == null || s2.Ten == null ? "" : s2.Ten.ToString()
						 });


            var bacSiDichVuListDtos = await query.ToListAsync();

            return _bacSiDichVusExcelExporter.ExportToFile(bacSiDichVuListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_BacSiDichVus)]
         public async Task<PagedResultDto<BacSiDichVuUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BacSiDichVuUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new BacSiDichVuUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<BacSiDichVuUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_BacSiDichVus)]
         public async Task<PagedResultDto<BacSiDichVuDichVuLookupTableDto>> GetAllDichVuForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_dichVuRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Ten != null && e.Ten.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var dichVuList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BacSiDichVuDichVuLookupTableDto>();
			foreach(var dichVu in dichVuList){
				lookupTableDtoList.Add(new BacSiDichVuDichVuLookupTableDto
				{
					Id = dichVu.Id,
					DisplayName = dichVu.Ten?.ToString()
				});
			}

            return new PagedResultDto<BacSiDichVuDichVuLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}