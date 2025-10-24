using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.HanetTenant.Exporting
{
    public class HanetTenantPlaceDatasesExcelExporter : NpoiExcelExporterBase, IHanetTenantPlaceDatasesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HanetTenantPlaceDatasesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHanetTenantPlaceDatasForViewDto> hanetTenantPlaceDatases)
        {
            return CreateExcelPackage(
                "HanetTenantPlaceDatases.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("HanetTenantPlaceDatases"));

                    AddHeader(
                        sheet,
                        L("placeName"),
                        L("placeAddress"),
                        L("placeId"),
                        L("userId"),
                        L("tenantId")
                        );

                    AddObjects(
                        sheet, 2, hanetTenantPlaceDatases,
                        _ => _.HanetTenantPlaceDatas.placeName,
                        _ => _.HanetTenantPlaceDatas.placeAddress,
                        _ => _.HanetTenantPlaceDatas.placeId,
                        _ => _.HanetTenantPlaceDatas.userId,
                        _ => _.HanetTenantPlaceDatas.tenantId
                        );

					
					
                });
        }
    }
}
