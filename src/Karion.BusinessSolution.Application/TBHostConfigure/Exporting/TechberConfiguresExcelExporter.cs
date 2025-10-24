using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.TBHostConfigure.Exporting
{
    public class TechberConfiguresExcelExporter : NpoiExcelExporterBase, ITechberConfiguresExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TechberConfiguresExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTechberConfigureForViewDto> techberConfigures)
        {
            return CreateExcelPackage(
                "TechberConfigures.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("TechberConfigures"));

                    AddHeader(
                        sheet,
                        L("Key"),
                        L("Value")
                        );

                    AddObjects(
                        sheet, 2, techberConfigures,
                        _ => _.TechberConfigure.Key,
                        _ => _.TechberConfigure.Value
                        );

					
					
                });
        }
    }
}
