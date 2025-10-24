using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Exporting
{
    public class LeaveRequestsExcelExporter : NpoiExcelExporterBase, ILeaveRequestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeaveRequestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeaveRequestForViewDto> leaveRequests)
        {
            return CreateExcelPackage(
                "LeaveRequests.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("LeaveRequests"));

                    AddHeader(
                        sheet,
                        L("StartDateTime"),
                        L("EndDateTime"),
                        L("LeaveType"),
                        L("Status"),
                        L("Reason"),
                        L("IsForEarlyLeave"),
                        L("IsForLateArrival"),
                        (L("NguoiBenh")) + L("UserName"),
                        (L("User")) + L("Name"),
                        (L("Shift")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, leaveRequests,
                        _ => _timeZoneConverter.Convert(_.LeaveRequest.StartDateTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.LeaveRequest.EndDateTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LeaveRequest.LeaveType,
                        _ => _.LeaveRequest.Status,
                        _ => _.LeaveRequest.Reason,
                        _ => _.LeaveRequest.IsForEarlyLeave,
                        _ => _.LeaveRequest.IsForLateArrival,
                        _ => _.NguoiBenhUserName,
                        _ => _.UserName,
                        _ => _.ShiftName
                        );

					
					for (var i = 1; i <= leaveRequests.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);for (var i = 1; i <= leaveRequests.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}
