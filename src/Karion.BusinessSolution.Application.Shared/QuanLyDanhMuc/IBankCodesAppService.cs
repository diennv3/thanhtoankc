using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IBankCodesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankCodeForViewDto>> GetAll(GetAllBankCodesInput input);

        Task<GetBankCodeForViewDto> GetBankCodeForView(int id);

		Task<GetBankCodeForEditOutput> GetBankCodeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBankCodeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBankCodesToExcel(GetAllBankCodesForExcelInput input);

		
    }
}