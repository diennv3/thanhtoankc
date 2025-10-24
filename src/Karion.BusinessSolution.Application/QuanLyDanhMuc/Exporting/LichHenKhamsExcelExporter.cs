using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class LichHenKhamsExcelExporter : NpoiExcelExporterBase, ILichHenKhamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LichHenKhamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLichHenKhamForViewDto> lichHenKhams)
        {
            return CreateExcelPackage(
                "LichHenKhams.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("LichHenKhams"));

                    AddHeader(
                        sheet,
                        L("NgayHenKham"),
                        L("MoTaTrieuChung"),
                        L("IsCoBHYT"),
                        L("SoTheBHYT"),
                        L("NoiDangKyKCBDauTien"),
                        L("BHYTValidDate"),
                        L("PhuongThucThanhToan"),
                        L("IsDaKham"),
                        L("IsDaThanhToan"),
                        L("TimeHoanThanhKham"),
                        L("TimeHoanThanhThanhToan"),
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("NguoiBenh")) + L("UserName"),
                        (L("NguoiThan")) + L("HoVaTen"),
                        (L("DichVu")) + L("Ten"),
                        (L("ChuyenKhoa")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, lichHenKhams,
                        _ => _timeZoneConverter.Convert(_.LichHenKham.NgayHenKham, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LichHenKham.MoTaTrieuChung,
                        _ => _.LichHenKham.IsCoBHYT,
                        _ => _.LichHenKham.SoTheBHYT,
                        _ => _.LichHenKham.NoiDangKyKCBDauTien,
                        _ => _timeZoneConverter.Convert(_.LichHenKham.BHYTValidDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LichHenKham.PhuongThucThanhToan,
                        _ => _.LichHenKham.IsDaKham,
                        _ => _.LichHenKham.IsDaThanhToan,
                        _ => _timeZoneConverter.Convert(_.LichHenKham.TimeHoanThanhKham, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.LichHenKham.TimeHoanThanhThanhToan, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.UserName2,
                        _ => _.NguoiBenhUserName,
                        _ => _.NguoiThanHoVaTen,
                        _ => _.ChuyenKhoaTen
                        );

					
					for (var i = 1; i <= lichHenKhams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);for (var i = 1; i <= lichHenKhams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(6);for (var i = 1; i <= lichHenKhams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[10], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(10);for (var i = 1; i <= lichHenKhams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[11], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(11);
                });
        }
    }
}
