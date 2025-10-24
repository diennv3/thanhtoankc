using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class DichVusExcelExporter : NpoiExcelExporterBase, IDichVusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DichVusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDichVuForViewDto> dichVus)
        {
            return CreateExcelPackage(
                "DichVus.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DichVus"));

                    AddHeader(
                        sheet,
                        L("Ten"),
                        L("MoTa"),
                        (L("ChuyenKhoa")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, dichVus,
                        _ => _.DichVu.Ten,
                        _ => _.DichVu.MoTa,
                        _ => _.ChuyenKhoaTen
                        );

					
					
                });
        }
    }
}
