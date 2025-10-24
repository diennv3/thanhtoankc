using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface INguoiThansAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNguoiThanForViewDto>> GetAll(GetAllNguoiThansInput input);

        Task<GetNguoiThanForViewDto> GetNguoiThanForView(int id);

		Task<GetNguoiThanForEditOutput> GetNguoiThanForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditNguoiThanDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetNguoiThansToExcel(GetAllNguoiThansForExcelInput input);

		
		Task<PagedResultDto<NguoiThanNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		
    }
}