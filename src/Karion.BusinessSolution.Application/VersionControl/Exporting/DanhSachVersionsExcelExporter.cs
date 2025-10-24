using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.VersionControl.Exporting
{
    public class DanhSachVersionsExcelExporter : NpoiExcelExporterBase, IDanhSachVersionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DanhSachVersionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDanhSachVersionForViewDto> danhSachVersions)
        {
            return CreateExcelPackage(
                "DanhSachVersions.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DanhSachVersions"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("VersionNumber"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, danhSachVersions,
                        _ => _.DanhSachVersion.Name,
                        _ => _.DanhSachVersion.VersionNumber,
                        _ => _.DanhSachVersion.IsActive
                        );

					
					
                });
        }
    }
}
