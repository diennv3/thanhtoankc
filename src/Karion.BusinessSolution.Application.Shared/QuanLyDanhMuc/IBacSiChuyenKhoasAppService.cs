using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization.Users.Profile.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IBacSiChuyenKhoasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBacSiChuyenKhoaForViewDto>> GetAll(GetAllBacSiChuyenKhoasInput input);

        Task<GetBacSiChuyenKhoaForViewDto> GetBacSiChuyenKhoaForView(int id);

		Task<GetBacSiChuyenKhoaForEditOutput> GetBacSiChuyenKhoaForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBacSiChuyenKhoaDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBacSiChuyenKhoasToExcel(GetAllBacSiChuyenKhoasForExcelInput input);

		
		Task<PagedResultDto<BacSiChuyenKhoaUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BacSiChuyenKhoaChuyenKhoaLookupTableDto>> GetAllChuyenKhoaForLookupTable(GetAllForLookupTableInput input);
		Task CapNhatAnhBacSi(CapNhatAnhBacSiInput input);

    }
}