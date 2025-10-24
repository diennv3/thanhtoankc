using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class ShiftsExcelExporter : NpoiExcelExporterBase, IShiftsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftForViewDto> shifts)
        {
            return CreateExcelPackage(
                "Shifts.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Shifts"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("StartTime"),
                        L("EndTime"),
                        L("WorkDaysMask"),
                        L("ToleranceMinutes"),
                        L("IsActive"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, shifts,
                        _ => _.Shift.Code,
                        _ => _.Shift.Name,
                        _ => _.Shift.StartTime.ToString(@"hh\:mm"),
                        _ => _.Shift.EndTime.ToString(@"hh\:mm"),
                        _ => _.Shift.WorkDaysMask,
                        _ => _.Shift.ToleranceMinutes,
                        _ => _.Shift.IsActive,
                        _ => _.Shift.Description
                        );

					
					for (var i = 1; i <= shifts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);for (var i = 1; i <= shifts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}
