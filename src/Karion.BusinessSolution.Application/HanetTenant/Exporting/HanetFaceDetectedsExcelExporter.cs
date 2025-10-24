using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.HanetTenant.Exporting
{
    public class HanetFaceDetectedsExcelExporter : NpoiExcelExporterBase, IHanetFaceDetectedsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HanetFaceDetectedsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHanetFaceDetectedForViewDto> hanetFaceDetecteds)
        {
            return CreateExcelPackage(
                "HanetFaceDetecteds.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("HanetFaceDetecteds"));

                    AddHeader(
                        sheet,
                        L("placeId"),
                        L("deviceId"),
                        L("userDetectedId"),
                        L("mask"),
                        L("detectImageUrl"),
                        L("flag")
                        );

                    AddObjects(
                        sheet, 2, hanetFaceDetecteds,
                        _ => _.HanetFaceDetected.placeId,
                        _ => _.HanetFaceDetected.deviceId,
                        _ => _.HanetFaceDetected.userDetectedId,
                        _ => _.HanetFaceDetected.mask,
                        _ => _.HanetFaceDetected.detectImageUrl,
                        _ => _.HanetFaceDetected.flag
                        );

					
					
                });
        }
    }
}
