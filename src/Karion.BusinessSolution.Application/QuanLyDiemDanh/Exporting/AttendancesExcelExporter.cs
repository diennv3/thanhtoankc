using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Exporting
{
    public class AttendancesExcelExporter : NpoiExcelExporterBase, IAttendancesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttendancesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttendanceForViewDto> attendances)
        {
            return CreateExcelPackage(
                "Attendances.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Attendances"));

                    AddHeader(
                        sheet,
                        L("CheckInLatitude"),
                        L("CheckInLongitude"),
                        L("IsCheckInFaceMatched"),
                        L("IsWithinLocation"),
                        L("CheckInDeviceInfo"),
                        L("PhotoPath"),
                        L("CheckInFaceMatchPercentage"),
                        L("CheckIn"),
                        L("IsLateCheckIn"),
                        L("IsOvertime"),
                        L("OvertimeStart"),
                        L("OvertimeEnd"),
                        (L("NguoiBenh")) + L("UserName")
                        );

                    AddObjects(
                        sheet, 2, attendances,
                        _ => _.Attendance.CheckInLatitude,
                        _ => _.Attendance.CheckInLongitude,
                        _ => _.Attendance.IsCheckInFaceMatched,
                        _ => _.Attendance.IsWithinLocation,
                        _ => _.Attendance.CheckInDeviceInfo,
                        _ => _.Attendance.PhotoPath,
                        _ => _.Attendance.CheckInFaceMatchPercentage,
                        _ => _timeZoneConverter.Convert(_.Attendance.CheckIn, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Attendance.IsLateCheckIn,
                        _ => _.Attendance.IsOvertime,
                        _ => _timeZoneConverter.Convert(_.Attendance.OvertimeStart, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Attendance.OvertimeEnd, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.NguoiBenhName
                        );

					
					for (var i = 1; i <= attendances.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(8);for (var i = 1; i <= attendances.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[11], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(11);for (var i = 1; i <= attendances.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[12], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(12);
                });
        }
    }
}
