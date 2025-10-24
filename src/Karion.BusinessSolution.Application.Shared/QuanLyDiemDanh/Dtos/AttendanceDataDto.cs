using System;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class AttendanceDataDto
    {
        public int NguoiBenhId { get; set; }
        public DateTime CheckIn { get; set; }
        public int? ShiftId { get; set; }
    }
}