﻿using Karion.BusinessSolution.QuanLyDanhMuc;
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
using Abp.UI;

namespace Karion.BusinessSolution.QuanLyDiemDanh
{
	[AbpAuthorize(AppPermissions.Pages_LeaveRequests)]
    public class LeaveRequestsAppService : BusinessSolutionAppServiceBase, ILeaveRequestsAppService
    {
		 private readonly IRepository<LeaveRequest> _leaveRequestRepository;
		 private readonly ILeaveRequestsExcelExporter _leaveRequestsExcelExporter;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 

		  public LeaveRequestsAppService(IRepository<LeaveRequest> leaveRequestRepository, ILeaveRequestsExcelExporter leaveRequestsExcelExporter , IRepository<NguoiBenh, int> lookup_nguoiBenhRepository) 
		  {
			_leaveRequestRepository = leaveRequestRepository;
			_leaveRequestsExcelExporter = leaveRequestsExcelExporter;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
		
		  }

		 public async Task<PagedResultDto<GetLeaveRequestForViewDto>> GetAll(GetAllLeaveRequestsInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (LeaveRequestStatus) input.StatusFilter
                        : default;			
					
			var filteredLeaveRequests = _leaveRequestRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
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
					
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter), e => e.ShiftFk != null && e.ShiftFk.Name == input.ShiftNameFilter);

			var pagedAndFilteredLeaveRequests = filteredLeaveRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var leaveRequests = from o in pagedAndFilteredLeaveRequests
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetLeaveRequestForViewDto() {
							LeaveRequest = new LeaveRequestDto
							{
                                StartDateTime = o.StartDateTime,
                                EndDateTime = o.EndDateTime,
                                Reason = o.Reason,
                                IsForEarlyLeave = o.IsForEarlyLeave,
                                IsForLateArrival = o.IsForLateArrival,
                                AllowedLateMinutes = o.AllowedLateMinutes,
                                AllowedEarlyMinutes = o.AllowedEarlyMinutes,
                                Type = o.Type,
                                HalfDayPart = o.HalfDayPart,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.HoVaTen == null ? "" : s1.HoVaTen.ToString(),
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
            if (input.StartDateTime > input.EndDateTime)
            {
                throw new UserFriendlyException("Thời gian bắt đầu phải nhỏ hơn hoặc bằng thời gian kết thúc");
            }

            if (input.IsForLateArrival && (!input.AllowedLateMinutes.HasValue || input.AllowedLateMinutes < 0))
                throw new UserFriendlyException("Vui lòng nhập số phút được phép đi muộn (>= 0)");

            if (input.IsForEarlyLeave && (!input.AllowedEarlyMinutes.HasValue || input.AllowedEarlyMinutes < 0))
                throw new UserFriendlyException("Vui lòng nhập số phút được phép về sớm (>= 0)");

            var leaveRequest = ObjectMapper.Map<LeaveRequest>(input);

            leaveRequest.DurationHours = (leaveRequest.EndDateTime - leaveRequest.StartDateTime).TotalHours;

            if (leaveRequest.Type == LeaveRequestType.EarlyLeave)
                leaveRequest.IsForEarlyLeave = true;
            else if (leaveRequest.Type == LeaveRequestType.LateArrival)
                leaveRequest.IsForLateArrival = true;

            if (leaveRequest.Status == LeaveRequestStatus.Approved && leaveRequest.NguoiBenhId.HasValue)
            {
                var hasOverlap = await _leaveRequestRepository.GetAll()
                    .Where(l => l.NguoiBenhId == leaveRequest.NguoiBenhId && l.Status == LeaveRequestStatus.Approved)
                    .AnyAsync(l => l.StartDateTime < leaveRequest.EndDateTime && l.EndDateTime > leaveRequest.StartDateTime);

                if (hasOverlap)
                    throw new UserFriendlyException("Đã tồn tại đơn phép đã được duyệt có thời gian chồng lấp. Vui lòng kiểm tra lại.");
            }

            if (leaveRequest.Status == LeaveRequestStatus.Approved)
            {
                leaveRequest.ApprovedAt = DateTime.Now;
                leaveRequest.ApprovedByUserId = AbpSession.UserId;
            }

            if (AbpSession.TenantId != null)
            {
                leaveRequest.TenantId = (int?)AbpSession.TenantId;
            }

            await _leaveRequestRepository.InsertAsync(leaveRequest);
        }

        [AbpAuthorize(AppPermissions.Pages_LeaveRequests_Edit)]
        protected virtual async Task Update(CreateOrEditLeaveRequestDto input)
        {
            if (input.StartDateTime > input.EndDateTime)
            {
                throw new UserFriendlyException("Thời gian bắt đầu phải nhỏ hơn hoặc bằng thời gian kết thúc");
            }

            if (input.IsForLateArrival && (!input.AllowedLateMinutes.HasValue || input.AllowedLateMinutes < 0))
                throw new UserFriendlyException("Vui lòng nhập số phút được phép đi muộn (>= 0)");

            if (input.IsForEarlyLeave && (!input.AllowedEarlyMinutes.HasValue || input.AllowedEarlyMinutes < 0))
                throw new UserFriendlyException("Vui lòng nhập số phút được phép về sớm (>= 0)");

            var leaveRequest = await _leaveRequestRepository.FirstOrDefaultAsync((int)input.Id);
            if (leaveRequest == null)
                throw new UserFriendlyException("Không tìm thấy đơn phép");

            var oldStatus = leaveRequest.Status;

            ObjectMapper.Map(input, leaveRequest);

            leaveRequest.DurationHours = (leaveRequest.EndDateTime - leaveRequest.StartDateTime).TotalHours;

            if (leaveRequest.Type == LeaveRequestType.EarlyLeave)
                leaveRequest.IsForEarlyLeave = true;
            else if (leaveRequest.Type == LeaveRequestType.LateArrival)
                leaveRequest.IsForLateArrival = true;

            if (leaveRequest.Status == LeaveRequestStatus.Approved && oldStatus != LeaveRequestStatus.Approved && leaveRequest.NguoiBenhId.HasValue)
            {
                var hasOverlap = await _leaveRequestRepository.GetAll()
                    .Where(l => l.NguoiBenhId == leaveRequest.NguoiBenhId && l.Status == LeaveRequestStatus.Approved && l.Id != leaveRequest.Id)
                    .AnyAsync(l => l.StartDateTime < leaveRequest.EndDateTime && l.EndDateTime > leaveRequest.StartDateTime);

                if (hasOverlap)
                    throw new UserFriendlyException("Đã tồn tại đơn phép đã được duyệt có thời gian chồng lấp. Vui lòng kiểm tra lại.");
            }

            if (leaveRequest.Status == LeaveRequestStatus.Approved && oldStatus != LeaveRequestStatus.Approved)
            {
                leaveRequest.ApprovedAt = DateTime.Now;
                leaveRequest.ApprovedByUserId = AbpSession.UserId;
            }

            await _leaveRequestRepository.UpdateAsync(leaveRequest);
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
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter), e => e.ShiftFk != null && e.ShiftFk.Name == input.ShiftNameFilter);

			var query = (from o in filteredLeaveRequests
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
				
                         select new GetLeaveRequestForViewDto() { 
							LeaveRequest = new LeaveRequestDto
							{
                                StartDateTime = o.StartDateTime,
                                EndDateTime = o.EndDateTime,
                                Status = o.Status,
                                Reason = o.Reason,
                                IsForEarlyLeave = o.IsForEarlyLeave,
                                IsForLateArrival = o.IsForLateArrival,
                                AllowedLateMinutes = o.AllowedLateMinutes,
                                AllowedEarlyMinutes = o.AllowedEarlyMinutes,
                                Type = o.Type,
                                HalfDayPart = o.HalfDayPart,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString(),
						 });


            var leaveRequestListDtos = await query.ToListAsync();

            return _leaveRequestsExcelExporter.ExportToFile(leaveRequestListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_LeaveRequests)]
         public async Task<PagedResultDto<LeaveRequestNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll()
	             .Where(x => x.IsNhanVien)
	             .WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.HoVaTen != null && e.HoVaTen.Contains(input.Filter)
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
					DisplayName = nguoiBenh.HoVaTen?.ToString() + " Tài khoản: " + nguoiBenh.UserName.ToString(), 
				});
			}

            return new PagedResultDto<LeaveRequestNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}