using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class PublicTokensExcelExporter : NpoiExcelExporterBase, IPublicTokensExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PublicTokensExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPublicTokenForViewDto> publicTokens)
        {
            return CreateExcelPackage(
                "PublicTokens.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PublicTokens"));

                    AddHeader(
                        sheet,
                        L("TimeSet"),
                        L("TimeExpire"),
                        L("Token"),
                        L("PrivateKey"),
                        L("DeviceVerificationCode"),
                        L("LastAccessDeviceVerificationCode"),
                        L("IsTokenLocked"),
                        (L("NguoiBenh")) + L("UserName")
                        );

                    AddObjects(
                        sheet, 2, publicTokens,
                        _ => _timeZoneConverter.Convert(_.PublicToken.TimeSet, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.PublicToken.TimeExpire, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.PublicToken.Token,
                        _ => _.PublicToken.PrivateKey,
                        _ => _.PublicToken.DeviceVerificationCode,
                        _ => _.PublicToken.LastAccessDeviceVerificationCode,
                        _ => _.PublicToken.IsTokenLocked,
                        _ => _.NguoiBenhUserName
                        );

					
					for (var i = 1; i <= publicTokens.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);for (var i = 1; i <= publicTokens.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}
