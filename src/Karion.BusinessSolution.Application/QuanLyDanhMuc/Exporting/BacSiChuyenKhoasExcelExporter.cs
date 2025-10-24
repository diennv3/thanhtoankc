using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class BacSiChuyenKhoasExcelExporter : NpoiExcelExporterBase, IBacSiChuyenKhoasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BacSiChuyenKhoasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBacSiChuyenKhoaForViewDto> bacSiChuyenKhoas)
        {
            return CreateExcelPackage(
                "BacSiChuyenKhoas.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("BacSiChuyenKhoas"));

                    AddHeader(
                        sheet,
                        (L("User")) + L("Name"),
                        (L("ChuyenKhoa")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, bacSiChuyenKhoas,
                        _ => _.UserName,
                        _ => _.ChuyenKhoaTen
                        );

					
					
                });
        }
    }
}
