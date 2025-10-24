using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.HanetTenant.Exporting
{
    public class HanetTenantLogsExcelExporter : NpoiExcelExporterBase, IHanetTenantLogsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HanetTenantLogsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHanetTenantLogForViewDto> hanetTenantLogs)
        {
            return CreateExcelPackage(
                "HanetTenantLogs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("HanetTenantLogs"));

                    AddHeader(
                        sheet,
                        L("Value")
                        );

                    AddObjects(
                        sheet, 2, hanetTenantLogs,
                        _ => _.HanetTenantLog.Value
                        );

					
					
                });
        }
    }
}
