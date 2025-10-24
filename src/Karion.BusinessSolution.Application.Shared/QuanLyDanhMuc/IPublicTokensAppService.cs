using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IPublicTokensAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPublicTokenForViewDto>> GetAll(GetAllPublicTokensInput input);

        Task<GetPublicTokenForViewDto> GetPublicTokenForView(long id);

		Task<GetPublicTokenForEditOutput> GetPublicTokenForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPublicTokenDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPublicTokensToExcel(GetAllPublicTokensForExcelInput input);

		
		Task<PagedResultDto<PublicTokenNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		
    }
}