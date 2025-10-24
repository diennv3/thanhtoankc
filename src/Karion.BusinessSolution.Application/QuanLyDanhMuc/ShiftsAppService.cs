

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
	[AbpAuthorize(AppPermissions.Pages_Shifts)]
    public class ShiftsAppService : BusinessSolutionAppServiceBase, IShiftsAppService
    {
		 private readonly IRepository<Shift> _shiftRepository;
		 private readonly IShiftsExcelExporter _shiftsExcelExporter;
		 

		  public ShiftsAppService(IRepository<Shift> shiftRepository, IShiftsExcelExporter shiftsExcelExporter ) 
		  {
			_shiftRepository = shiftRepository;
			_shiftsExcelExporter = shiftsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetShiftForViewDto>> GetAll(GetAllShiftsInput input)
         {
			
			var filteredShifts = _shiftRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinStartTimeFilter != null, e => e.StartTime >= input.MinStartTimeFilter)
						.WhereIf(input.MaxStartTimeFilter != null, e => e.StartTime <= input.MaxStartTimeFilter)
						.WhereIf(input.MinEndTimeFilter != null, e => e.EndTime >= input.MinEndTimeFilter)
						.WhereIf(input.MaxEndTimeFilter != null, e => e.EndTime <= input.MaxEndTimeFilter)
						.WhereIf(input.MinWorkDaysMaskFilter != null, e => e.WorkDaysMask >= input.MinWorkDaysMaskFilter)
						.WhereIf(input.MaxWorkDaysMaskFilter != null, e => e.WorkDaysMask <= input.MaxWorkDaysMaskFilter)
						.WhereIf(input.MinToleranceMinutesFilter != null, e => e.ToleranceMinutes >= input.MinToleranceMinutesFilter)
						.WhereIf(input.MaxToleranceMinutesFilter != null, e => e.ToleranceMinutes <= input.MaxToleranceMinutesFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var pagedAndFilteredShifts = filteredShifts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shifts = from o in pagedAndFilteredShifts
                         select new GetShiftForViewDto() {
							Shift = new ShiftDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                StartTime = o.StartTime,
                                EndTime = o.EndTime,
                                WorkDaysMask = o.WorkDaysMask,
                                ToleranceMinutes = o.ToleranceMinutes,
                                IsActive = o.IsActive,
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredShifts.CountAsync();

            return new PagedResultDto<GetShiftForViewDto>(
                totalCount,
                await shifts.ToListAsync()
            );
         }
		 
		 public async Task<GetShiftForViewDto> GetShiftForView(int id)
         {
            var shift = await _shiftRepository.GetAsync(id);

            var output = new GetShiftForViewDto { Shift = ObjectMapper.Map<ShiftDto>(shift) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Shifts_Edit)]
		 public async Task<GetShiftForEditOutput> GetShiftForEdit(EntityDto input)
         {
            var shift = await _shiftRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShiftForEditOutput {Shift = ObjectMapper.Map<CreateOrEditShiftDto>(shift)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Create)]
		 protected virtual async Task Create(CreateOrEditShiftDto input)
         {
            var shift = ObjectMapper.Map<Shift>(input);

			
			if (AbpSession.TenantId != null)
			{
				shift.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _shiftRepository.InsertAsync(shift);
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftDto input)
         {
            var shift = await _shiftRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, shift);
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input)
         {
			
			var filteredShifts = _shiftRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinStartTimeFilter != null, e => e.StartTime >= input.MinStartTimeFilter)
						.WhereIf(input.MaxStartTimeFilter != null, e => e.StartTime <= input.MaxStartTimeFilter)
						.WhereIf(input.MinEndTimeFilter != null, e => e.EndTime >= input.MinEndTimeFilter)
						.WhereIf(input.MaxEndTimeFilter != null, e => e.EndTime <= input.MaxEndTimeFilter)
						.WhereIf(input.MinWorkDaysMaskFilter != null, e => e.WorkDaysMask >= input.MinWorkDaysMaskFilter)
						.WhereIf(input.MaxWorkDaysMaskFilter != null, e => e.WorkDaysMask <= input.MaxWorkDaysMaskFilter)
						.WhereIf(input.MinToleranceMinutesFilter != null, e => e.ToleranceMinutes >= input.MinToleranceMinutesFilter)
						.WhereIf(input.MaxToleranceMinutesFilter != null, e => e.ToleranceMinutes <= input.MaxToleranceMinutesFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var query = (from o in filteredShifts
                         select new GetShiftForViewDto() { 
							Shift = new ShiftDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                StartTime = o.StartTime,
                                EndTime = o.EndTime,
                                WorkDaysMask = o.WorkDaysMask,
                                ToleranceMinutes = o.ToleranceMinutes,
                                IsActive = o.IsActive,
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var shiftListDtos = await query.ToListAsync();

            return _shiftsExcelExporter.ExportToFile(shiftListDtos);
         }

        public async Task<List<ShiftDto>> GetAllActiveForSelect()
        {
            var list = await _shiftRepository.GetAll()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new ShiftDto
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name = s.Name,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    WorkDaysMask = s.WorkDaysMask,
                    ToleranceMinutes = s.ToleranceMinutes,
                    IsActive = s.IsActive,
                    Description = s.Description
                })
                .ToListAsync();

            return list;
        }
    }
}