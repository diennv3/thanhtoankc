using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDiemDanh
{
    public interface IAttendancesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAttendanceForViewDto>> GetAll(GetAllAttendancesInput input);

        Task<GetAttendanceForViewDto> GetAttendanceForView(int id);

		Task<GetAttendanceForEditOutput> GetAttendanceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAttendanceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAttendancesToExcel(GetAllAttendancesForExcelInput input);

		
		Task<PagedResultDto<AttendanceNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetNhanVienNguoiBenhDto>> GetNhanVienNguoiBenh();

		Task<BaoCaoTongHopResultDto> BaoCaoTongHop(BaoCaoTheoKyInputDto input);
    }
}