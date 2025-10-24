using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class ThongTinDonViesExcelExporter : NpoiExcelExporterBase, IThongTinDonViesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ThongTinDonViesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetThongTinDonViForViewDto> thongTinDonVies)
        {
            return CreateExcelPackage(
                "ThongTinDonVies.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ThongTinDonVies"));

                    AddHeader(
                        sheet,
                        L("Key"),
                        L("Value")
                        );

                    AddObjects(
                        sheet, 2, thongTinDonVies,
                        _ => _.ThongTinDonVi.Key,
                        _ => _.ThongTinDonVi.Value
                        );

					
					
                });
        }
    }
}
