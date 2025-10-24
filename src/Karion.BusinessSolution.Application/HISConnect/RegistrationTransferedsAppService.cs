using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.HISConnect.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.HISConnect
{
	[AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds)]
    public class RegistrationTransferedsAppService : BusinessSolutionAppServiceBase, IRegistrationTransferedsAppService
    {
		 private readonly IRepository<RegistrationTransfered, long> _registrationTransferedRepository;
		 private readonly IRepository<LichHenKham,int> _lookup_lichHenKhamRepository;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 

		  public RegistrationTransferedsAppService(IRepository<RegistrationTransfered, long> registrationTransferedRepository , IRepository<LichHenKham, int> lookup_lichHenKhamRepository, IRepository<NguoiBenh, int> lookup_nguoiBenhRepository) 
		  {
			_registrationTransferedRepository = registrationTransferedRepository;
			_lookup_lichHenKhamRepository = lookup_lichHenKhamRepository;
		_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
		
		  }

		 public async Task<PagedResultDto<GetRegistrationTransferedForViewDto>> GetAll(GetAllRegistrationTransferedsInput input)
         {
			
			var filteredRegistrationTransfereds = _registrationTransferedRepository.GetAll()
						.Include( e => e.LichHenKhamFk)
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinApprovedDateFilter != null, e => e.ApprovedDate >= input.MinApprovedDateFilter)
						.WhereIf(input.MaxApprovedDateFilter != null, e => e.ApprovedDate <= input.MaxApprovedDateFilter)
						.WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
						.WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LichHenKhamMoTaTrieuChungFilter), e => e.LichHenKhamFk != null && e.LichHenKhamFk.MoTaTrieuChung == input.LichHenKhamMoTaTrieuChungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var pagedAndFilteredRegistrationTransfereds = filteredRegistrationTransfereds
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var registrationTransfereds = from o in pagedAndFilteredRegistrationTransfereds
                         join o1 in _lookup_lichHenKhamRepository.GetAll() on o.LichHenKhamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRegistrationTransferedForViewDto() {
							RegistrationTransfered = new RegistrationTransferedDto
							{
                                ApprovedDate = o.ApprovedDate,
                                RegistrationDate = o.RegistrationDate,
                                Id = o.Id
							},
                         	LichHenKhamMoTaTrieuChung = s1 == null || s1.MoTaTrieuChung == null ? "" : s1.MoTaTrieuChung.ToString(),
                         	NguoiBenhUserName = s2 == null || s2.UserName == null ? "" : s2.UserName.ToString()
						};

            var totalCount = await filteredRegistrationTransfereds.CountAsync();

            return new PagedResultDto<GetRegistrationTransferedForViewDto>(
                totalCount,
                await registrationTransfereds.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds_Edit)]
		 public async Task<GetRegistrationTransferedForEditOutput> GetRegistrationTransferedForEdit(EntityDto<long> input)
         {
            var registrationTransfered = await _registrationTransferedRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRegistrationTransferedForEditOutput {RegistrationTransfered = ObjectMapper.Map<CreateOrEditRegistrationTransferedDto>(registrationTransfered)};

		    if (output.RegistrationTransfered.LichHenKhamId != null)
            {
                var _lookupLichHenKham = await _lookup_lichHenKhamRepository.FirstOrDefaultAsync((int)output.RegistrationTransfered.LichHenKhamId);
                output.LichHenKhamMoTaTrieuChung = _lookupLichHenKham?.MoTaTrieuChung?.ToString();
            }

		    if (output.RegistrationTransfered.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.RegistrationTransfered.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRegistrationTransferedDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds_Create)]
		 protected virtual async Task Create(CreateOrEditRegistrationTransferedDto input)
         {
            var registrationTransfered = ObjectMapper.Map<RegistrationTransfered>(input);

			
			if (AbpSession.TenantId != null)
			{
				registrationTransfered.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _registrationTransferedRepository.InsertAsync(registrationTransfered);
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds_Edit)]
		 protected virtual async Task Update(CreateOrEditRegistrationTransferedDto input)
         {
            var registrationTransfered = await _registrationTransferedRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, registrationTransfered);
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _registrationTransferedRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds)]
         public async Task<PagedResultDto<RegistrationTransferedLichHenKhamLookupTableDto>> GetAllLichHenKhamForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_lichHenKhamRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.MoTaTrieuChung != null && e.MoTaTrieuChung.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var lichHenKhamList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RegistrationTransferedLichHenKhamLookupTableDto>();
			foreach(var lichHenKham in lichHenKhamList){
				lookupTableDtoList.Add(new RegistrationTransferedLichHenKhamLookupTableDto
				{
					Id = lichHenKham.Id,
					DisplayName = lichHenKham.MoTaTrieuChung?.ToString()
				});
			}

            return new PagedResultDto<RegistrationTransferedLichHenKhamLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_RegistrationTransfereds)]
         public async Task<PagedResultDto<RegistrationTransferedNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RegistrationTransferedNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new RegistrationTransferedNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<RegistrationTransferedNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}