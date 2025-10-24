using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class NguoiBenhNotificationsExcelExporter : NpoiExcelExporterBase, INguoiBenhNotificationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NguoiBenhNotificationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNguoiBenhNotificationForViewDto> nguoiBenhNotifications)
        {
            return CreateExcelPackage(
                "NguoiBenhNotifications.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("NguoiBenhNotifications"));

                    AddHeader(
                        sheet,
                        L("NoiDungTinNhan"),
                        L("TrangThai"),
                        L("TieuDe"),
                        L("ThoiGianGui"),
                        (L("NguoiBenh")) + L("UserName")
                        );

                    AddObjects(
                        sheet, 2, nguoiBenhNotifications,
                        _ => _.NguoiBenhNotification.NoiDungTinNhan,
                        _ => _.NguoiBenhNotification.TrangThai,
                        _ => _.NguoiBenhNotification.TieuDe,
                        _ => _timeZoneConverter.Convert(_.NguoiBenhNotification.ThoiGianGui, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.NguoiBenhUserName
                        );

					
					for (var i = 1; i <= nguoiBenhNotifications.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}
