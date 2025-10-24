using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.VersionControl.Exporting
{
    public class VersionsExcelExporter : NpoiExcelExporterBase, IVersionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public VersionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetVersionForViewDto> versions)
        {
            return CreateExcelPackage(
                "Versions.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Versions"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Version"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, versions,
                        _ => _.Version.Name,
                        _ => _.Version.Version,
                        _ => _.Version.IsActive
                        );

					
					
                });
        }
    }
}
