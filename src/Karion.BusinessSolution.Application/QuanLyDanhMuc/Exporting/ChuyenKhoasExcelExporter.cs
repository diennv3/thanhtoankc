using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class ChuyenKhoasExcelExporter : NpoiExcelExporterBase, IChuyenKhoasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChuyenKhoasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChuyenKhoaForViewDto> chuyenKhoas)
        {
            return CreateExcelPackage(
                "ChuyenKhoas.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ChuyenKhoas"));

                    AddHeader(
                        sheet,
                        L("Ten"),
                        L("MoTa")
                        );

                    AddObjects(
                        sheet, 2, chuyenKhoas,
                        _ => _.ChuyenKhoa.Ten,
                        _ => _.ChuyenKhoa.MoTa
                        );

					
					
                });
        }
    }
}
