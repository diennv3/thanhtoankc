using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class ChiTietThanhToansExcelExporter : NpoiExcelExporterBase, IChiTietThanhToansExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChiTietThanhToansExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChiTietThanhToanForViewDto> chiTietThanhToans)
        {
            return CreateExcelPackage(
                "ChiTietThanhToans.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ChiTietThanhToans"));

                    AddHeader(
                        sheet,
                        L("SoTienThanhToan"),
                        L("LoaiThanhToan"),
                        L("NgayThanhToan"),
                        (L("LichHenKham")) + L("MoTaTrieuChung"),
                        (L("NguoiBenh")) + L("UserName")
                        );

                    AddObjects(
                        sheet, 2, chiTietThanhToans,
                        _ => _.ChiTietThanhToan.SoTienThanhToan,
                        _ => _.ChiTietThanhToan.LoaiThanhToan,
                        _ => _timeZoneConverter.Convert(_.ChiTietThanhToan.NgayThanhToan, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LichHenKhamMoTaTrieuChung,
                        _ => _.NguoiBenhUserName
                        );

					
					for (var i = 1; i <= chiTietThanhToans.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}
