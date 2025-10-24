using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class GiaDichVusExcelExporter : NpoiExcelExporterBase, IGiaDichVusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GiaDichVusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGiaDichVuForViewDto> giaDichVus)
        {
            return CreateExcelPackage(
                "GiaDichVus.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("GiaDichVus"));

                    AddHeader(
                        sheet,
                        L("MucGia"),
                        L("MoTa"),
                        L("Gia"),
                        L("NgayApDung"),
                        (L("DichVu")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, giaDichVus,
                        _ => _.GiaDichVu.MucGia,
                        _ => _.GiaDichVu.MoTa,
                        _ => _.GiaDichVu.Gia,
                        _ => _timeZoneConverter.Convert(_.GiaDichVu.NgayApDung, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.DichVuTen
                        );

					
					for (var i = 1; i <= giaDichVus.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}
