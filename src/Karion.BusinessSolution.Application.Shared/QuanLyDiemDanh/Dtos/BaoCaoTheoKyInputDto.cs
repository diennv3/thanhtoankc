using System;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class BaoCaoTheoKyInputDto
    {
        public long? NguoiBenhId { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public LOAI_BAO_CAO LoaiBaoCao { get; set; } // "QUY", "THANG", "NAM"
    }
}