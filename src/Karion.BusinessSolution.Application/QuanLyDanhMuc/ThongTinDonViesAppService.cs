

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
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_ThongTinDonVies)]
    public class ThongTinDonViesAppService : BusinessSolutionAppServiceBase, IThongTinDonViesAppService
    {
		 private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;
		 private readonly IRepository<BankCode> _bankCodeRepository;
		 private readonly IThongTinDonViesExcelExporter _thongTinDonViesExcelExporter;
		 

		  public ThongTinDonViesAppService(IRepository<ThongTinDonVi> thongTinDonViRepository, IThongTinDonViesExcelExporter thongTinDonViesExcelExporter,
			  IRepository<BankCode> bankCodeRepository
			  ) 
		  {
			_thongTinDonViRepository = thongTinDonViRepository;
			_thongTinDonViesExcelExporter = thongTinDonViesExcelExporter;
			_bankCodeRepository = bankCodeRepository;
		  }

		 public async Task<PagedResultDto<GetThongTinDonViForViewDto>> GetAll(GetAllThongTinDonViesInput input)
         {
			
			var filteredThongTinDonVies = _thongTinDonViRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Key.Contains(input.Filter) || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyFilter),  e => e.Key == input.KeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var pagedAndFilteredThongTinDonVies = filteredThongTinDonVies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var thongTinDonVies = from o in pagedAndFilteredThongTinDonVies
                         select new GetThongTinDonViForViewDto() {
							ThongTinDonVi = new ThongTinDonViDto
							{
                                Key = o.Key,
                                Value = o.Value,
                                Id = o.Id
							}
						};

            var totalCount = await filteredThongTinDonVies.CountAsync();

            return new PagedResultDto<GetThongTinDonViForViewDto>(
                totalCount,
                await thongTinDonVies.ToListAsync()
            );
         }
		 
		 public async Task<GetThongTinDonViForViewDto> GetThongTinDonViForView(int id)
         {
            var thongTinDonVi = await _thongTinDonViRepository.GetAsync(id);

            var output = new GetThongTinDonViForViewDto { ThongTinDonVi = ObjectMapper.Map<ThongTinDonViDto>(thongTinDonVi) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ThongTinDonVies_Edit)]
		 public async Task<GetThongTinDonViForEditOutput> GetThongTinDonViForEdit(EntityDto input)
         {
            var thongTinDonVi = await _thongTinDonViRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetThongTinDonViForEditOutput {ThongTinDonVi = ObjectMapper.Map<CreateOrEditThongTinDonViDto>(thongTinDonVi)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditThongTinDonViDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ThongTinDonVies_Create)]
		 protected virtual async Task Create(CreateOrEditThongTinDonViDto input)
         {
            var thongTinDonVi = ObjectMapper.Map<ThongTinDonVi>(input);

			
			if (AbpSession.TenantId != null)
			{
				thongTinDonVi.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _thongTinDonViRepository.InsertAsync(thongTinDonVi);
         }

		 [AbpAuthorize(AppPermissions.Pages_ThongTinDonVies_Edit)]
		 protected virtual async Task Update(CreateOrEditThongTinDonViDto input)
         {
            var thongTinDonVi = await _thongTinDonViRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, thongTinDonVi);
         }

		 [AbpAuthorize(AppPermissions.Pages_ThongTinDonVies_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _thongTinDonViRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetThongTinDonViesToExcel(GetAllThongTinDonViesForExcelInput input)
         {
			
			var filteredThongTinDonVies = _thongTinDonViRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Key.Contains(input.Filter) || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyFilter),  e => e.Key == input.KeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var query = (from o in filteredThongTinDonVies
                         select new GetThongTinDonViForViewDto() { 
							ThongTinDonVi = new ThongTinDonViDto
							{
                                Key = o.Key,
                                Value = o.Value,
                                Id = o.Id
							}
						 });


            var thongTinDonViListDtos = await query.ToListAsync();

            return _thongTinDonViesExcelExporter.ExportToFile(thongTinDonViListDtos);
         }

		public async Task<GetTenantBankInfoDto> GetTenantBankInfo()
		{
			var bankCode = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("BankCode"));
			var bankAccount = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("BankAccount"));
			return new GetTenantBankInfoDto()
			{
				BankAccount = bankAccount.isNull()?"":bankAccount.Value,
				BankCode = bankCode.isNull()?"":bankCode.Value,
				
			};
		}
		public async Task<List<BankCodeDto>> GetListBankCode()
		{
			return ObjectMapper.Map<List<BankCodeDto>>(await _bankCodeRepository.GetAllListAsync());
		}
    }
}