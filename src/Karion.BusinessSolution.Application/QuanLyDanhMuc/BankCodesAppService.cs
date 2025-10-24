

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
	[AbpAuthorize(AppPermissions.Pages_BankCodes)]
    public class BankCodesAppService : BusinessSolutionAppServiceBase, IBankCodesAppService
    {
		 private readonly IRepository<BankCode> _bankCodeRepository;
		 private readonly IBankCodesExcelExporter _bankCodesExcelExporter;
		 

		  public BankCodesAppService(IRepository<BankCode> bankCodeRepository, IBankCodesExcelExporter bankCodesExcelExporter ) 
		  {
			_bankCodeRepository = bankCodeRepository;
			_bankCodesExcelExporter = bankCodesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetBankCodeForViewDto>> GetAll(GetAllBankCodesInput input)
         {
			
			var filteredBankCodes = _bankCodeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.BankName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter),  e => e.BankName == input.BankNameFilter);

			var pagedAndFilteredBankCodes = filteredBankCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bankCodes = from o in pagedAndFilteredBankCodes
                         select new GetBankCodeForViewDto() {
							BankCode = new BankCodeDto
							{
                                Code = o.Code,
                                BankName = o.BankName,
                                Id = o.Id
							}
						};

            var totalCount = await filteredBankCodes.CountAsync();

            return new PagedResultDto<GetBankCodeForViewDto>(
                totalCount,
                await bankCodes.ToListAsync()
            );
         }
		 
		 public async Task<GetBankCodeForViewDto> GetBankCodeForView(int id)
         {
            var bankCode = await _bankCodeRepository.GetAsync(id);

            var output = new GetBankCodeForViewDto { BankCode = ObjectMapper.Map<BankCodeDto>(bankCode) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BankCodes_Edit)]
		 public async Task<GetBankCodeForEditOutput> GetBankCodeForEdit(EntityDto input)
         {
            var bankCode = await _bankCodeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBankCodeForEditOutput {BankCode = ObjectMapper.Map<CreateOrEditBankCodeDto>(bankCode)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBankCodeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BankCodes_Create)]
		 protected virtual async Task Create(CreateOrEditBankCodeDto input)
         {
            var bankCode = ObjectMapper.Map<BankCode>(input);

			

            await _bankCodeRepository.InsertAsync(bankCode);
         }

		 [AbpAuthorize(AppPermissions.Pages_BankCodes_Edit)]
		 protected virtual async Task Update(CreateOrEditBankCodeDto input)
         {
            var bankCode = await _bankCodeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, bankCode);
         }

		 [AbpAuthorize(AppPermissions.Pages_BankCodes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bankCodeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBankCodesToExcel(GetAllBankCodesForExcelInput input)
         {
			
			var filteredBankCodes = _bankCodeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.BankName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter),  e => e.BankName == input.BankNameFilter);

			var query = (from o in filteredBankCodes
                         select new GetBankCodeForViewDto() { 
							BankCode = new BankCodeDto
							{
                                Code = o.Code,
                                BankName = o.BankName,
                                Id = o.Id
							}
						 });


            var bankCodeListDtos = await query.ToListAsync();

            return _bankCodesExcelExporter.ExportToFile(bankCodeListDtos);
         }


    }
}