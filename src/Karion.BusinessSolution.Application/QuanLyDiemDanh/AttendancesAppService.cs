using Karion.BusinessSolution.QuanLyDanhMuc;


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
using Abp.UI;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.QuanLyDiemDanh
{
	[AbpAuthorize(AppPermissions.Pages_Attendances)]
    public class AttendancesAppService : BusinessSolutionAppServiceBase, IAttendancesAppService
    {
		 private readonly IRepository<Attendance> _attendanceRepository;
		 private readonly IAttendancesExcelExporter _attendancesExcelExporter;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;
		 private readonly IRepository<Shift> _shiftRepository;
		 

		  public AttendancesAppService(IRepository<Attendance> attendanceRepository, IRepository<Shift> shiftRepository, IRepository<ThongTinDonVi> thongTinDonViRepository, IAttendancesExcelExporter attendancesExcelExporter , IRepository<NguoiBenh, int> lookup_nguoiBenhRepository) 
		  {
			_attendanceRepository = attendanceRepository;
			_thongTinDonViRepository = thongTinDonViRepository;
			_attendancesExcelExporter = attendancesExcelExporter;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
			_shiftRepository = shiftRepository;
		  }

		 public async Task<PagedResultDto<GetAttendanceForViewDto>> GetAll(GetAllAttendancesInput input)
         {
			
			var filteredAttendances = _attendanceRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CheckInDeviceInfo.Contains(input.Filter))
						.WhereIf(input.MinCheckInFilter != null, e => e.CheckIn >= input.MinCheckInFilter)
						.WhereIf(input.MaxCheckInFilter != null, e => e.CheckIn <= input.MaxCheckInFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var pagedAndFilteredAttendances = filteredAttendances
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var attendances = from o in pagedAndFilteredAttendances
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttendanceForViewDto() {
							Attendance = new AttendanceDto
							{
                                CheckInLatitude = o.CheckInLatitude,
                                CheckInLongitude = o.CheckInLongitude,
                                IsCheckInFaceMatched = o.IsCheckInFaceMatched,
                                IsWithinLocation = o.IsWithinLocation,
                                CheckInDeviceInfo = o.CheckInDeviceInfo,
                                PhotoPath = o.PhotoPath,
                                CheckInFaceMatchPercentage = o.CheckInFaceMatchPercentage,
                                CheckIn = o.CheckIn,
                                IsLateCheckIn = o.IsLateCheckIn,
                                IsOvertime = o.IsOvertime,
                                OvertimeStart = o.OvertimeStart,
                                OvertimeEnd = o.OvertimeEnd,
                                Id = o.Id
							},
                         	NguoiBenhName = s1 == null || s1.HoVaTen == null ? "" : s1.HoVaTen.ToString()
						};

            var totalCount = await filteredAttendances.CountAsync();

            return new PagedResultDto<GetAttendanceForViewDto>(
                totalCount,
                await attendances.ToListAsync()
            );
         }
		 
		 public async Task<GetAttendanceForViewDto> GetAttendanceForView(int id)
         {
            var attendance = await _attendanceRepository.GetAsync(id);

            var output = new GetAttendanceForViewDto { Attendance = ObjectMapper.Map<AttendanceDto>(attendance) };

		    if (output.Attendance.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.Attendance.NguoiBenhId);
                output.NguoiBenhName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Attendances_Edit)]
		 public async Task<GetAttendanceForEditOutput> GetAttendanceForEdit(EntityDto input)
         {
            var attendance = await _attendanceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAttendanceForEditOutput {Attendance = ObjectMapper.Map<CreateOrEditAttendanceDto>(attendance)};

		    if (output.Attendance.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.Attendance.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAttendanceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Attendances_Create)]
		 protected virtual async Task Create(CreateOrEditAttendanceDto input)
         {
            var attendance = ObjectMapper.Map<Attendance>(input);

			
			if (AbpSession.TenantId != null)
			{
				attendance.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _attendanceRepository.InsertAsync(attendance);
         }

		 [AbpAuthorize(AppPermissions.Pages_Attendances_Edit)]
		 protected virtual async Task Update(CreateOrEditAttendanceDto input)
         {
            var attendance = await _attendanceRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, attendance);
         }

		 [AbpAuthorize(AppPermissions.Pages_Attendances_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _attendanceRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAttendancesToExcel(GetAllAttendancesForExcelInput input)
         {
			
			var filteredAttendances = _attendanceRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CheckInDeviceInfo.Contains(input.Filter) || e.PhotoPath.Contains(input.Filter))
						.WhereIf(input.MinCheckInLatitudeFilter != null, e => e.CheckInLatitude >= input.MinCheckInLatitudeFilter)
						.WhereIf(input.MaxCheckInLatitudeFilter != null, e => e.CheckInLatitude <= input.MaxCheckInLatitudeFilter)
						.WhereIf(input.MinCheckInLongitudeFilter != null, e => e.CheckInLongitude >= input.MinCheckInLongitudeFilter)
						.WhereIf(input.MaxCheckInLongitudeFilter != null, e => e.CheckInLongitude <= input.MaxCheckInLongitudeFilter)
						.WhereIf(input.IsCheckInFaceMatchedFilter > -1,  e => (input.IsCheckInFaceMatchedFilter == 1 && e.IsCheckInFaceMatched) || (input.IsCheckInFaceMatchedFilter == 0 && !e.IsCheckInFaceMatched) )
						.WhereIf(input.IsWithinLocationFilter > -1,  e => (input.IsWithinLocationFilter == 1 && e.IsWithinLocation) || (input.IsWithinLocationFilter == 0 && !e.IsWithinLocation) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CheckInDeviceInfoFilter),  e => e.CheckInDeviceInfo == input.CheckInDeviceInfoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhotoPathFilter),  e => e.PhotoPath == input.PhotoPathFilter)
						.WhereIf(input.MinCheckInFaceMatchPercentageFilter != null, e => e.CheckInFaceMatchPercentage >= input.MinCheckInFaceMatchPercentageFilter)
						.WhereIf(input.MaxCheckInFaceMatchPercentageFilter != null, e => e.CheckInFaceMatchPercentage <= input.MaxCheckInFaceMatchPercentageFilter)
						.WhereIf(input.MinCheckInFilter != null, e => e.CheckIn >= input.MinCheckInFilter)
						.WhereIf(input.MaxCheckInFilter != null, e => e.CheckIn <= input.MaxCheckInFilter)
						.WhereIf(input.IsLateCheckInFilter > -1,  e => (input.IsLateCheckInFilter == 1 && e.IsLateCheckIn) || (input.IsLateCheckInFilter == 0 && !e.IsLateCheckIn) )
						.WhereIf(input.IsOvertimeFilter > -1,  e => (input.IsOvertimeFilter == 1 && e.IsOvertime) || (input.IsOvertimeFilter == 0 && !e.IsOvertime) )
						.WhereIf(input.MinOvertimeStartFilter != null, e => e.OvertimeStart >= input.MinOvertimeStartFilter)
						.WhereIf(input.MaxOvertimeStartFilter != null, e => e.OvertimeStart <= input.MaxOvertimeStartFilter)
						.WhereIf(input.MinOvertimeEndFilter != null, e => e.OvertimeEnd >= input.MinOvertimeEndFilter)
						.WhereIf(input.MaxOvertimeEndFilter != null, e => e.OvertimeEnd <= input.MaxOvertimeEndFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var query = (from o in filteredAttendances
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttendanceForViewDto() { 
							Attendance = new AttendanceDto
							{
                                CheckInLatitude = o.CheckInLatitude,
                                CheckInLongitude = o.CheckInLongitude,
                                IsCheckInFaceMatched = o.IsCheckInFaceMatched,
                                IsWithinLocation = o.IsWithinLocation,
                                CheckInDeviceInfo = o.CheckInDeviceInfo,
                                PhotoPath = o.PhotoPath,
                                CheckInFaceMatchPercentage = o.CheckInFaceMatchPercentage,
                                CheckIn = o.CheckIn,
                                IsLateCheckIn = o.IsLateCheckIn,
                                IsOvertime = o.IsOvertime,
                                OvertimeStart = o.OvertimeStart,
                                OvertimeEnd = o.OvertimeEnd,
                                Id = o.Id
							},
							NguoiBenhName = s1 == null || s1.HoVaTen == null ? "" : s1.HoVaTen.ToString()
						 });


            var attendanceListDtos = await query.ToListAsync();

            return _attendancesExcelExporter.ExportToFile(attendanceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Attendances)]
         public async Task<PagedResultDto<AttendanceNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AttendanceNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new AttendanceNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<AttendanceNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
         public async Task<List<GetNhanVienNguoiBenhDto>> GetNhanVienNguoiBenh()
         {
	         return await _lookup_nguoiBenhRepository.GetAll()
		         .Where(x => x.IsNhanVien)
		         .Select(x => new GetNhanVienNguoiBenhDto
		         {
			         Id = x.Id,
			         Ten = x.HoVaTen
		         }).ToListAsync();
         }
        public async Task<BaoCaoTongHopResultDto> BaoCaoTongHop(BaoCaoTheoKyInputDto input)
		{
		    var fromDate = input.TuNgay?.Date ?? DateTime.Now.Date.AddMonths(-1);
		    var toDate = input.DenNgay?.Date ?? DateTime.Now.Date;

		    var nguoiBenhQuery = _lookup_nguoiBenhRepository.GetAll().Where(x => x.IsNhanVien);

		    if (input.NguoiBenhId.HasValue)
		        nguoiBenhQuery = nguoiBenhQuery.Where(x => x.Id == input.NguoiBenhId.Value);

		    var nhanViens = await nguoiBenhQuery
		        .Select(x => new { x.Id, x.HoVaTen })
		        .ToListAsync();

		    var nhanVienIds = nhanViens.Select(x => x.Id).ToList();

		    var thongTinDonVi = await _thongTinDonViRepository.GetAll()
		        .Where(x => x.Key == "GioLamViecSang" || x.Key == "GioLamViecChieu" || x.Key == "ThuBay" || x.Key == "ChuNhat" || x.Key == "WorkDays")
		        .ToListAsync();

		    var gioSangParts = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecSang")?.Value?.Split('-');
		    var gioChieuParts = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecChieu")?.Value?.Split('-');
		    if (gioSangParts == null || gioChieuParts == null)
		        throw new UserFriendlyException("Chưa cấu hình giờ làm việc");

		    TimeSpan startMorning = TimeSpan.Parse(gioSangParts[0]);
		    TimeSpan endMorning = TimeSpan.Parse(gioSangParts[1]);
		    TimeSpan startAfternoon = TimeSpan.Parse(gioChieuParts[0]);
		    TimeSpan endAfternoon = TimeSpan.Parse(gioChieuParts[1]);

		    string valSat = thongTinDonVi.FirstOrDefault(x => x.Key == "ThuBay")?.Value?.ToLower();
		    bool isWorkingSaturday = valSat == "true" || valSat == "1";
		    string valSun = thongTinDonVi.FirstOrDefault(x => x.Key == "ChuNhat")?.Value?.ToLower();
		    bool isWorkingSunday = valSun == "true" || valSun == "1";

		    var allDates = Enumerable.Range(0, (toDate - fromDate).Days + 1)
		        .Select(offset => fromDate.AddDays(offset)).ToList();

		    var allShifts = await _shiftRepository.GetAll().ToListAsync();
		    var shiftDict = allShifts.ToDictionary(s => s.Id, s => s);

		    var attendanceData = await _attendanceRepository.GetAll()
		        .Where(x => nhanVienIds.Contains(x.NguoiBenhId.Value) && x.CheckIn >= fromDate.AddDays(-1) && x.CheckIn <= toDate.AddDays(1).AddDays(1))
		        .Select(a => new AttendanceDataDto
		        { 
		            NguoiBenhId = a.NguoiBenhId.Value, 
		            CheckIn = a.CheckIn,
		            ShiftId = a.ShiftId
		        })
		        .ToListAsync();

		    var attendancesByEmployee = attendanceData
		        .GroupBy(a => a.NguoiBenhId)
		        .ToDictionary(g => g.Key, g => g.OrderBy(x => x.CheckIn).ToList());

		    var result = new BaoCaoTongHopResultDto();
		    result.NhanVienReports = new List<BaoCaoTongHopNhanVienDto>();

		    foreach (var nv in nhanViens)
		    {
		        attendancesByEmployee.TryGetValue(nv.Id, out var empChecks);
		        empChecks = empChecks ?? new List<AttendanceDataDto>();

		        var chiTietKy = new List<ChamCongKyDto>();
		        int tongLam = 0, tongNghi = 0, soDiTre = 0, soVeSom = 0;

		        foreach (var date in allDates)
		        {
		            var isSaturday = date.DayOfWeek == DayOfWeek.Saturday;
		            var isSunday = date.DayOfWeek == DayOfWeek.Sunday;
		            bool isWorkingDay = true;
		            if (isSaturday) isWorkingDay = isWorkingSaturday;
		            else if (isSunday) isWorkingDay = isWorkingSunday;

		            if (!isWorkingDay)
		            {
		                chiTietKy.Add(new ChamCongKyDto
		                {
		                    Ky = date.ToString("yyyy-MM-dd"),
		                    TrangThai = "Nghỉ (theo cấu hình)",
		                    DiTrePhut = 0,
		                    VeSomPhut = 0,
		                    ShiftType = "Hành chính",
		                    ShiftName = "Hành chính",
		                    OvertimeHours = 0
		                });
		                continue;
		            }

		            var attendanceForDate = empChecks
		                .FirstOrDefault(a => a.CheckIn.Date == date);

		            Shift empShift = null;
		            string shiftName = "Hành chính";
		            string shiftType = "Hành chính";

		            if (attendanceForDate?.ShiftId.HasValue == true && 
		                shiftDict.TryGetValue(attendanceForDate.ShiftId.Value, out var attendanceShift))
		            {
		                empShift = attendanceShift;
		                shiftName = string.IsNullOrWhiteSpace(empShift.Name) ? "Ca" : empShift.Name;
		                shiftType = "Ca";
		            }

		            TimeSpan shiftStart, shiftEnd;
		            int toleranceMinutes = 15;

		            if (empShift != null)
		            {
		                shiftStart = empShift.StartTime;
		                shiftEnd = empShift.EndTime;
		                toleranceMinutes = empShift.ToleranceMinutes;
		            }
		            else
		            {
		                shiftStart = startMorning;
		                shiftEnd = endAfternoon;
		                toleranceMinutes = 15;
		            }

		            DateTime shiftStartDt, shiftEndDt;
		            if (shiftStart <= shiftEnd)
		            {
		                shiftStartDt = date.Date + shiftStart;
		                shiftEndDt = date.Date + shiftEnd;
		            }
		            else
		            {
		                shiftStartDt = date.Date + shiftStart;
		                shiftEndDt = date.Date.AddDays(1) + shiftEnd;
		            }

		            var checksInWindow = empChecks
		                .Where(a => a.CheckIn >= shiftStartDt && a.CheckIn <= shiftEndDt)
		                .OrderBy(a => a.CheckIn)
		                .ToList();

		            if (!checksInWindow.Any())
		            {
		                tongNghi++;
		                chiTietKy.Add(new ChamCongKyDto
		                {
		                    Ky = date.ToString("yyyy-MM-dd"),
		                    TrangThai = "Nghỉ",
		                    DiTrePhut = 0,
		                    VeSomPhut = 0,
		                    ShiftType = shiftType,
		                    ShiftName = shiftName,
		                    OvertimeHours = 0
		                });
		                continue;
		            }

		            tongLam++;

		            DateTime first = checksInWindow.First().CheckIn;
		            DateTime last = checksInWindow.Last().CheckIn;

		            int diTrePhut = 0;
		            int veSomPhut = 0;
		            double overtimeHours = 0;

		            if (shiftType == "Hành chính")
		            {
		                if (first.TimeOfDay > startMorning && first.TimeOfDay <= endMorning)
		                    diTrePhut = (int)(first.TimeOfDay - startMorning).TotalMinutes;

		                if (last.TimeOfDay < endAfternoon && last.TimeOfDay >= startAfternoon)
		                    veSomPhut = (int)(endAfternoon - last.TimeOfDay).TotalMinutes;

		                if (last > shiftEndDt)
		                {
		                    overtimeHours = (last - shiftEndDt).TotalHours;
		                }
		            }
		            else
		            {
		                if (first.TimeOfDay > shiftStart && first.TimeOfDay <= (shiftStart + TimeSpan.FromHours(12))) // reasonable upper bound
		                    diTrePhut = (int)(first.TimeOfDay - shiftStart).TotalMinutes;

		                if (last.TimeOfDay < shiftEnd && last.TimeOfDay >= (shiftEnd - TimeSpan.FromHours(12)))
		                    veSomPhut = (int)(shiftEnd - last.TimeOfDay).TotalMinutes;

		                if (last > shiftEndDt)
		                    overtimeHours = (last - shiftEndDt).TotalHours;
		            }

		            if (diTrePhut > 0) soDiTre++;
		            if (veSomPhut > 0) soVeSom++;

		            chiTietKy.Add(new ChamCongKyDto
		            {
		                Ky = date.ToString("yyyy-MM-dd"),
		                TrangThai = "Làm việc",
		                DiTrePhut = diTrePhut,
		                VeSomPhut = veSomPhut,
		                ShiftType = shiftType,
		                ShiftName = shiftName,
		                OvertimeHours = Math.Round(overtimeHours, 2)
		            });
		        }

		        result.NhanVienReports.Add(new BaoCaoTongHopNhanVienDto
		        {
		            NhanVienId = nv.Id,
		            TenNhanVien = nv.HoVaTen,
		            ChiTietKy = chiTietKy,
		            TongNgayLam = tongLam,
		            TongNgayNghi = tongNghi,
		            SoNgayDiTre = soDiTre,
		            SoNgayVeSom = soVeSom
		        });
		    }

		    result.TongHop = new BaoCaoTongHopNhanVienDto
		    {
		        NhanVienId = null,
		        TenNhanVien = "Tất cả",
		        TongNgayLam = result.NhanVienReports.Sum(x => x.TongNgayLam),
		        TongNgayNghi = result.NhanVienReports.Sum(x => x.TongNgayNghi),
		        SoNgayDiTre = result.NhanVienReports.Sum(x => x.SoNgayDiTre),
		        SoNgayVeSom = result.NhanVienReports.Sum(x => x.SoNgayVeSom)
		    };

		    return result;
		}
    }
}