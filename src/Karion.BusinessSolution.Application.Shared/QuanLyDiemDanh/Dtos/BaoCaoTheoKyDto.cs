using System;
using System.Collections.Generic;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class ChamCongKyDto
    {
        public string Ky { get; set; }
        public string TrangThai { get; set; }
        public int DiTrePhut { get; set; }
        public int VeSomPhut { get; set; }

        // Mới: loại phân ca (Ca / Hành chính)
        public string ShiftType { get; set; }

        // Mới: tên phân ca (tên trong bảng Shift hoặc "Hành chính")
        public string ShiftName { get; set; }

        // Mới: số giờ tăng ca (nếu có)
        public double OvertimeHours { get; set; }
    }

    public class BaoCaoTongHopNhanVienDto
    {
        public int? NhanVienId { get; set; }
        public string TenNhanVien { get; set; }
        public List<ChamCongKyDto> ChiTietKy { get; set; }
        public int TongNgayLam { get; set; }
        public int TongNgayNghi { get; set; }
        public int SoNgayDiTre { get; set; }
        public int SoNgayVeSom { get; set; }
    }

    public class BaoCaoTongHopResultDto
    {
        public List<BaoCaoTongHopNhanVienDto> NhanVienReports { get; set; }
        public BaoCaoTongHopNhanVienDto TongHop { get; set; }
    }
}