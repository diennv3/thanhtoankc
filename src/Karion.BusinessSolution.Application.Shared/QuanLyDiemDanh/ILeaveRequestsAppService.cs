using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDiemDanh
{
    public interface ILeaveRequestsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLeaveRequestForViewDto>> GetAll(GetAllLeaveRequestsInput input);

        Task<GetLeaveRequestForViewDto> GetLeaveRequestForView(int id);

		Task<GetLeaveRequestForEditOutput> GetLeaveRequestForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLeaveRequestDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLeaveRequestsToExcel(GetAllLeaveRequestsForExcelInput input);

		
		Task<PagedResultDto<LeaveRequestNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		
    }
}