using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.HanetTenant.Exporting
{
    public class HanetTenantDeviceDatasesExcelExporter : NpoiExcelExporterBase, IHanetTenantDeviceDatasesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HanetTenantDeviceDatasesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHanetTenantDeviceDatasForViewDto> hanetTenantDeviceDatases)
        {
            return CreateExcelPackage(
                "HanetTenantDeviceDatases.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("HanetTenantDeviceDatases"));

                    AddHeader(
                        sheet,
                        L("deviceId"),
                        L("deviceName"),
                        L("deviceStatus"),
                        L("lastCheck"),
                        L("tenantId"),
                        (L("HanetTenantPlaceDatas")) + L("placeName")
                        );

                    AddObjects(
                        sheet, 2, hanetTenantDeviceDatases,
                        _ => _.HanetTenantDeviceDatas.deviceId,
                        _ => _.HanetTenantDeviceDatas.deviceName,
                        _ => _.HanetTenantDeviceDatas.deviceStatus,
                        _ => _timeZoneConverter.Convert(_.HanetTenantDeviceDatas.lastCheck, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.HanetTenantDeviceDatas.tenantId,
                        _ => _.HanetTenantPlaceDatasplaceName
                        );

					
					for (var i = 1; i <= hanetTenantDeviceDatases.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}
