using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using System.Collections.Generic;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IShiftsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftForViewDto>> GetAll(GetAllShiftsInput input);

        Task<GetShiftForViewDto> GetShiftForView(int id);

		Task<GetShiftForEditOutput> GetShiftForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input);

        Task<List<ShiftDto>> GetAllActiveForSelect();
    }
}