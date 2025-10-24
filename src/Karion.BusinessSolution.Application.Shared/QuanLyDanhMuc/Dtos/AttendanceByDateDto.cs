using System;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class AttendanceDailyStatusDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? MorningCheckIn { get; set; }
        public int? MorningLateMinutes { get; set; }
        public DateTime? AfternoonCheckIn { get; set; }
        public int? AfternoonEarlyLeaveMinutes { get; set; }
    }
}