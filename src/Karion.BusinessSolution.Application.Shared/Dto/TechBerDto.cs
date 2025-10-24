using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

namespace Karion.BusinessSolution.Dto
{
    public class CommonLookupTableDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class LichHenKhamAppServiceConst
    {
        public const int LE_TAN_FLAG = 1;
        public const int BAC_SI_CHI_DINH_FLAG = 2;
        public const int THU_NGAN_FLAG = 3;
    }

    public class GetKhungKhamDto
    {
        public DateTime NgayHenKham { get; set; }
        public int ChuyenKhoaId { get; set; }
    }
    public class GetKhungKhamResultDto
    {
       public List<LichHenKhamDto> LichHenKham { get; set; }
       public int KhamSession {get; set;}
       public string GioBatDauLamViecSang {get; set;}
       public string GioKetThucLamViecSang {get; set;} 
       public string GioBatDauLamViecChieu {get; set;}
       public string GioKetThucLamViecChieu {get; set;}
    }

    public class ThanhToanVienPhiDto
    {
        public int Id { get; set; }
        public decimal SoTienThanhToan { get; set; }
    }

    public class LichHenKhamForViewDto
    {
        public LichHenKhamDto LichHenKham { get; set; }
        public NguoiBenhDto NguoiBenh { get; set; }
    }

    public class GeneratorQRDto
    {
        public int LichHenKhamId { get; set; }
        public int ChiTietThanhToanId { get; set; }
    }
    public class CapNhatAnhBacSiInput
    {
        [Required]
        [MaxLength(400)]
        public string FileToken { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
        public long ThongTinBacSiMoRongId { get; set; }
    }

    public class KiemTraThanhToanNganHangDto
    {
        public int lichHenKhamId { get; set; }
    }

    public class MBBankGetTransactionHistoryDto
    {
        public int lichHenKhamId { get; set; }
    }

    public class ResponseKiemTraThanhToanMBBank
    {
        public string message { get; set; }
        public bool isUpdateTrangThaiThanhToan { get; set; }
        public int chiTietThanhToanId { get; set; }
    }
}