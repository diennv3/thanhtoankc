using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class BacSiDichVusExcelExporter : NpoiExcelExporterBase, IBacSiDichVusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BacSiDichVusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBacSiDichVuForViewDto> bacSiDichVus)
        {
            return CreateExcelPackage(
                "BacSiDichVus.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("BacSiDichVus"));

                    AddHeader(
                        sheet,
                        (L("User")) + L("Name"),
                        (L("DichVu")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, bacSiDichVus,
                        _ => _.UserName,
                        _ => _.DichVuTen
                        );

					
					
                });
        }
    }
}
