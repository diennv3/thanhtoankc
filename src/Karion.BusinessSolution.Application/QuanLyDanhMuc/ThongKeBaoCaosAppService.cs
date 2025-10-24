using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.QuanLyDiemDanh;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public class ThongKeBaoCaosAppService : BusinessSolutionAppServiceBase, IThongKeBaoCaosAppService
    {
        private readonly IRepository<Attendance> _diemDanhRepository;
        private readonly IRepository<NguoiBenh> _nguoiBenhRepository;
        private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;

        public ThongKeBaoCaosAppService(
            IRepository<Attendance> diemDanhRepository,
            IRepository<NguoiBenh> nguoiBenhRepository,
            IRepository<ThongTinDonVi> thongTinDonViRepository
        )
        {
            _diemDanhRepository = diemDanhRepository;
            _nguoiBenhRepository = nguoiBenhRepository;
            _thongTinDonViRepository = thongTinDonViRepository;
        }

        public async Task<List<AttendanceDailyStatusDto>> GetDailyAttendanceStatus(DateTime date)
        {
            var thongTinDonVi = await _thongTinDonViRepository.GetAll()
                .Where(x => x.Key == "GioLamViecSang" || x.Key == "GioLamViecChieu")
                .ToListAsync();

            var gioSangInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecSang")?.Value ?? "07:00-11:30";
            var gioChieuInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecChieu")?.Value ?? "13:00-17:30";
            var gioSangParts = gioSangInfo.Split('-');
            var gioChieuParts = gioChieuInfo.Split('-');
            TimeSpan startMorning = TimeSpan.Parse(gioSangParts[0]);
            TimeSpan endMorning = TimeSpan.Parse(gioSangParts[1]);
            TimeSpan startAfternoon = TimeSpan.Parse(gioChieuParts[0]);
            TimeSpan endAfternoon = TimeSpan.Parse(gioChieuParts[1]);

            var allUsers = await _nguoiBenhRepository.GetAll()
                .Where(u => u.IsNhanVien == true)
                .Select(u => new { Id = u.Id, Name = u.HoVaTen })
                .ToListAsync();

            var attendances = await _diemDanhRepository.GetAll()
                .Where(x => x.CheckIn.Date == date.Date)
                .ToListAsync();

            var result = new List<AttendanceDailyStatusDto>();

            foreach (var user in allUsers)
            {
                var caSang = attendances
                    .Where(x => x.NguoiBenhId == user.Id && x.CheckIn.TimeOfDay >= startMorning && x.CheckIn.TimeOfDay <= endMorning)
                    .OrderBy(x => x.CheckIn)
                    .FirstOrDefault();

                var caChieu = attendances
                    .Where(x => x.NguoiBenhId == user.Id && x.CheckIn.TimeOfDay >= startAfternoon && x.CheckIn.TimeOfDay <= endAfternoon)
                    .OrderByDescending(x => x.CheckIn) // lấy lần cuối trong ca chiều
                    .FirstOrDefault();

                int? morningLate = null;
                if (caSang != null)
                {
                    var lateMinutes = (int)(caSang.CheckIn.TimeOfDay - startMorning).TotalMinutes;
                    morningLate = lateMinutes > 0 ? lateMinutes : 0;
                }

                int? afternoonEarly = null;
                if (caChieu != null)
                {
                    var earlyMinutes = (int)(endAfternoon - caChieu.CheckIn.TimeOfDay).TotalMinutes;
                    afternoonEarly = earlyMinutes > 0 ? earlyMinutes : 0;
                }

                result.Add(new AttendanceDailyStatusDto
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    MorningCheckIn = caSang?.CheckIn,
                    MorningLateMinutes = morningLate,
                    AfternoonCheckIn = caChieu?.CheckIn,
                    AfternoonEarlyLeaveMinutes = afternoonEarly
                });
            }

            return result;
        }
    }
}