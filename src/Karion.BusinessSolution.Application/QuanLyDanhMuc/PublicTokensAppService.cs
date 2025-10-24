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
	[AbpAuthorize(AppPermissions.Pages_PublicTokens)]
    public class PublicTokensAppService : BusinessSolutionAppServiceBase, IPublicTokensAppService
    {
		 private readonly IRepository<PublicToken, long> _publicTokenRepository;
		 private readonly IPublicTokensExcelExporter _publicTokensExcelExporter;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 

		  public PublicTokensAppService(IRepository<PublicToken, long> publicTokenRepository, IPublicTokensExcelExporter publicTokensExcelExporter , IRepository<NguoiBenh, int> lookup_nguoiBenhRepository) 
		  {
			_publicTokenRepository = publicTokenRepository;
			_publicTokensExcelExporter = publicTokensExcelExporter;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
		
		  }

		 public async Task<PagedResultDto<GetPublicTokenForViewDto>> GetAll(GetAllPublicTokensInput input)
         {
			
			var filteredPublicTokens = _publicTokenRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Token.Contains(input.Filter) || e.PrivateKey.Contains(input.Filter) || e.DeviceVerificationCode.Contains(input.Filter) || e.LastAccessDeviceVerificationCode.Contains(input.Filter))
						.WhereIf(input.MinTimeSetFilter != null, e => e.TimeSet >= input.MinTimeSetFilter)
						.WhereIf(input.MaxTimeSetFilter != null, e => e.TimeSet <= input.MaxTimeSetFilter)
						.WhereIf(input.MinTimeExpireFilter != null, e => e.TimeExpire >= input.MinTimeExpireFilter)
						.WhereIf(input.MaxTimeExpireFilter != null, e => e.TimeExpire <= input.MaxTimeExpireFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TokenFilter),  e => e.Token == input.TokenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PrivateKeyFilter),  e => e.PrivateKey == input.PrivateKeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DeviceVerificationCodeFilter),  e => e.DeviceVerificationCode == input.DeviceVerificationCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastAccessDeviceVerificationCodeFilter),  e => e.LastAccessDeviceVerificationCode == input.LastAccessDeviceVerificationCodeFilter)
						.WhereIf(input.IsTokenLockedFilter > -1,  e => (input.IsTokenLockedFilter == 1 && e.IsTokenLocked) || (input.IsTokenLockedFilter == 0 && !e.IsTokenLocked) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var pagedAndFilteredPublicTokens = filteredPublicTokens
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var publicTokens = from o in pagedAndFilteredPublicTokens
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPublicTokenForViewDto() {
							PublicToken = new PublicTokenDto
							{
                                TimeSet = o.TimeSet,
                                TimeExpire = o.TimeExpire,
                                Token = o.Token,
                                PrivateKey = o.PrivateKey,
                                DeviceVerificationCode = o.DeviceVerificationCode,
                                LastAccessDeviceVerificationCode = o.LastAccessDeviceVerificationCode,
                                IsTokenLocked = o.IsTokenLocked,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString()
						};

            var totalCount = await filteredPublicTokens.CountAsync();

            return new PagedResultDto<GetPublicTokenForViewDto>(
                totalCount,
                await publicTokens.ToListAsync()
            );
         }
		 
		 public async Task<GetPublicTokenForViewDto> GetPublicTokenForView(long id)
         {
            var publicToken = await _publicTokenRepository.GetAsync(id);

            var output = new GetPublicTokenForViewDto { PublicToken = ObjectMapper.Map<PublicTokenDto>(publicToken) };

		    if (output.PublicToken.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.PublicToken.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PublicTokens_Edit)]
		 public async Task<GetPublicTokenForEditOutput> GetPublicTokenForEdit(EntityDto<long> input)
         {
            var publicToken = await _publicTokenRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPublicTokenForEditOutput {PublicToken = ObjectMapper.Map<CreateOrEditPublicTokenDto>(publicToken)};

		    if (output.PublicToken.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.PublicToken.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPublicTokenDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PublicTokens_Create)]
		 protected virtual async Task Create(CreateOrEditPublicTokenDto input)
         {
            var publicToken = ObjectMapper.Map<PublicToken>(input);

			
			if (AbpSession.TenantId != null)
			{
				publicToken.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _publicTokenRepository.InsertAsync(publicToken);
         }

		 [AbpAuthorize(AppPermissions.Pages_PublicTokens_Edit)]
		 protected virtual async Task Update(CreateOrEditPublicTokenDto input)
         {
            var publicToken = await _publicTokenRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, publicToken);
         }

		 [AbpAuthorize(AppPermissions.Pages_PublicTokens_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _publicTokenRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPublicTokensToExcel(GetAllPublicTokensForExcelInput input)
         {
			
			var filteredPublicTokens = _publicTokenRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Token.Contains(input.Filter) || e.PrivateKey.Contains(input.Filter) || e.DeviceVerificationCode.Contains(input.Filter) || e.LastAccessDeviceVerificationCode.Contains(input.Filter))
						.WhereIf(input.MinTimeSetFilter != null, e => e.TimeSet >= input.MinTimeSetFilter)
						.WhereIf(input.MaxTimeSetFilter != null, e => e.TimeSet <= input.MaxTimeSetFilter)
						.WhereIf(input.MinTimeExpireFilter != null, e => e.TimeExpire >= input.MinTimeExpireFilter)
						.WhereIf(input.MaxTimeExpireFilter != null, e => e.TimeExpire <= input.MaxTimeExpireFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TokenFilter),  e => e.Token == input.TokenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PrivateKeyFilter),  e => e.PrivateKey == input.PrivateKeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DeviceVerificationCodeFilter),  e => e.DeviceVerificationCode == input.DeviceVerificationCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastAccessDeviceVerificationCodeFilter),  e => e.LastAccessDeviceVerificationCode == input.LastAccessDeviceVerificationCodeFilter)
						.WhereIf(input.IsTokenLockedFilter > -1,  e => (input.IsTokenLockedFilter == 1 && e.IsTokenLocked) || (input.IsTokenLockedFilter == 0 && !e.IsTokenLocked) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var query = (from o in filteredPublicTokens
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPublicTokenForViewDto() { 
							PublicToken = new PublicTokenDto
							{
                                TimeSet = o.TimeSet,
                                TimeExpire = o.TimeExpire,
                                Token = o.Token,
                                PrivateKey = o.PrivateKey,
                                DeviceVerificationCode = o.DeviceVerificationCode,
                                LastAccessDeviceVerificationCode = o.LastAccessDeviceVerificationCode,
                                IsTokenLocked = o.IsTokenLocked,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString()
						 });


            var publicTokenListDtos = await query.ToListAsync();

            return _publicTokensExcelExporter.ExportToFile(publicTokenListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PublicTokens)]
         public async Task<PagedResultDto<PublicTokenNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PublicTokenNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new PublicTokenNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<PublicTokenNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}