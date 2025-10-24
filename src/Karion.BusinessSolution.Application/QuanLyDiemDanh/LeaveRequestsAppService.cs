using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.QuanLyDanhMuc;

using Karion.BusinessSolution.QuanLyDiemDanh;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDiemDanh.Exporting;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.QuanLyDiemDanh
{
	[AbpAuthorize(AppPermissions.Pages_LeaveRequests)]
    public class LeaveRequestsAppService : BusinessSolutionAppServiceBase, ILeaveRequestsAppService
    {
		 private readonly IRepository<LeaveRequest> _leaveRequestRepository;
		 private readonly ILeaveRequestsExcelExporter _leaveRequestsExcelExporter;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Shift,int> _lookup_shiftRepository;
		 

		  public LeaveRequestsAppService(IRepository<LeaveRequest> leaveRequestRepository, ILeaveRequestsExcelExporter leaveRequestsExcelExporter , IRepository<NguoiBenh, int> lookup_nguoiBenhRepository, IRepository<User, long> lookup_userRepository, IRepository<Shift, int> lookup_shiftRepository) 
		  {
			_leaveRequestRepository = leaveRequestRepository;
			_leaveRequestsExcelExporter = leaveRequestsExcelExporter;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
		_lookup_userRepository = lookup_userRepository;
		_lookup_shiftRepository = lookup_shiftRepository;
		
		  }

		 public async Task<PagedResultDto<GetLeaveRequestForViewDto>> GetAll(GetAllLeaveRequestsInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (LeaveRequestStatus) input.StatusFilter
                        : default;			
					
			var filteredLeaveRequests = _leaveRequestRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.Include( e => e.UserFk)
						.Include( e => e.ShiftFk)
						
						.WhereIf(input.MinStartDateTimeFilter != null, e => e.StartDateTime >= input.MinStartDateTimeFilter)
						.WhereIf(input.MaxStartDateTimeFilter != null, e => e.StartDateTime <= input.MaxStartDateTimeFilter)
						.WhereIf(input.MinEndDateTimeFilter != null, e => e.EndDateTime >= input.MinEndDateTimeFilter)
						.WhereIf(input.MaxEndDateTimeFilter != null, e => e.EndDateTime <= input.MaxEndDateTimeFilter)
						.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReasonFilter),  e => e.Reason == input.ReasonFilter)
						.WhereIf(input.IsForEarlyLeaveFilter > -1,  e => (input.IsForEarlyLeaveFilter == 1 && e.IsForEarlyLeave) || (input.IsForEarlyLeaveFilter == 0 && !e.IsForEarlyLeave) )
						.WhereIf(input.IsForLateArrivalFilter > -1,  e => (input.IsForLateArrivalFilter == 1 && e.IsForLateArrival) || (input.IsForLateArrivalFilter == 0 && !e.IsForLateArrival) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter), e => e.ShiftFk != null && e.ShiftFk.Name == input.ShiftNameFilter);

			var pagedAndFilteredLeaveRequests = filteredLeaveRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var leaveRequests = from o in pagedAndFilteredLeaveRequests
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetLeaveRequestForViewDto() {
							LeaveRequest = new LeaveRequestDto
							{
                                StartDateTime = o.StartDateTime,
                                EndDateTime = o.EndDateTime,
                                Status = o.Status,
                                Reason = o.Reason,
                                IsForEarlyLeave = o.IsForEarlyLeave,
                                IsForLateArrival = o.IsForLateArrival,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                         	ShiftName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredLeaveRequests.CountAsync();

            return new PagedResultDto<GetLeaveRequestForViewDto>(
                totalCount,
                await leaveRequests.ToListAsync()
            );
         }
		 
		 public async Task<GetLeaveRequestForViewDto> GetLeaveRequestForView(int id)
         {
            var leaveRequest = await _leaveRequestRepository.GetAsync(id);

            var output = new GetLeaveRequestForViewDto { LeaveRequest = ObjectMapper.Map<LeaveRequestDto>(leaveRequest) };

		    if (output.LeaveRequest.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.LeaveRequest.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }

		    if (output.LeaveRequest.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LeaveRequest.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.LeaveRequest.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.LeaveRequest.ShiftId);
                output.ShiftName = _lookupShift?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LeaveRequests_Edit)]
		 public async Task<GetLeaveRequestForEditOutput> GetLeaveRequestForEdit(EntityDto input)
         {
            var leaveRequest = await _leaveRequestRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLeaveRequestForEditOutput {LeaveRequest = ObjectMapper.Map<CreateOrEditLeaveRequestDto>(leaveRequest)};

		    if (output.LeaveRequest.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.LeaveRequest.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }

		    if (output.LeaveRequest.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LeaveRequest.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.LeaveRequest.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.LeaveRequest.ShiftId);
                output.ShiftName = _lookupShift?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLeaveRequestDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LeaveRequests_Create)]
		 protected virtual async Task Create(CreateOrEditLeaveRequestDto input)
         {
            var leaveRequest = ObjectMapper.Map<LeaveRequest>(input);

			
			if (AbpSession.TenantId != null)
			{
				leaveRequest.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _leaveRequestRepository.InsertAsync(leaveRequest);
         }

		 [AbpAuthorize(AppPermissions.Pages_LeaveRequests_Edit)]
		 protected virtual async Task Update(CreateOrEditLeaveRequestDto input)
         {
            var leaveRequest = await _leaveRequestRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, leaveRequest);
         }

		 [AbpAuthorize(AppPermissions.Pages_LeaveRequests_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _leaveRequestRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLeaveRequestsToExcel(GetAllLeaveRequestsForExcelInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (LeaveRequestStatus) input.StatusFilter
                        : default;			
					
			var filteredLeaveRequests = _leaveRequestRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.Include( e => e.UserFk)
						.Include( e => e.ShiftFk)
						
						.WhereIf(input.MinStartDateTimeFilter != null, e => e.StartDateTime >= input.MinStartDateTimeFilter)
						.WhereIf(input.MaxStartDateTimeFilter != null, e => e.StartDateTime <= input.MaxStartDateTimeFilter)
						.WhereIf(input.MinEndDateTimeFilter != null, e => e.EndDateTime >= input.MinEndDateTimeFilter)
						.WhereIf(input.MaxEndDateTimeFilter != null, e => e.EndDateTime <= input.MaxEndDateTimeFilter)
						.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReasonFilter),  e => e.Reason == input.ReasonFilter)
						.WhereIf(input.IsForEarlyLeaveFilter > -1,  e => (input.IsForEarlyLeaveFilter == 1 && e.IsForEarlyLeave) || (input.IsForEarlyLeaveFilter == 0 && !e.IsForEarlyLeave) )
						.WhereIf(input.IsForLateArrivalFilter > -1,  e => (input.IsForLateArrivalFilter == 1 && e.IsForLateArrival) || (input.IsForLateArrivalFilter == 0 && !e.IsForLateArrival) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter), e => e.ShiftFk != null && e.ShiftFk.Name == input.ShiftNameFilter);

			var query = (from o in filteredLeaveRequests
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetLeaveRequestForViewDto() { 
							LeaveRequest = new LeaveRequestDto
							{
                                StartDateTime = o.StartDateTime,
                                EndDateTime = o.EndDateTime,
                                Status = o.Status,
                                Reason = o.Reason,
                                IsForEarlyLeave = o.IsForEarlyLeave,
                                IsForLateArrival = o.IsForLateArrival,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                         	ShiftName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
						 });


            var leaveRequestListDtos = await query.ToListAsync();

            return _leaveRequestsExcelExporter.ExportToFile(leaveRequestListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_LeaveRequests)]
         public async Task<PagedResultDto<LeaveRequestNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LeaveRequestNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new LeaveRequestNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<LeaveRequestNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_LeaveRequests)]
         public async Task<PagedResultDto<LeaveRequestUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LeaveRequestUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new LeaveRequestUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<LeaveRequestUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_LeaveRequests)]
         public async Task<PagedResultDto<LeaveRequestShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LeaveRequestShiftLookupTableDto>();
			foreach(var shift in shiftList){
				lookupTableDtoList.Add(new LeaveRequestShiftLookupTableDto
				{
					Id = shift.Id,
					DisplayName = shift.Name?.ToString()
				});
			}

            return new PagedResultDto<LeaveRequestShiftLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}