using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class NguoiBenhsExcelExporter : NpoiExcelExporterBase, INguoiBenhsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NguoiBenhsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNguoiBenhForViewDto> nguoiBenhs)
        {
            return CreateExcelPackage(
                "NguoiBenhs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("NguoiBenhs"));

                    AddHeader(
                        sheet,
                        L("HoVaTen"),
                        L("Tuoi"),
                        L("GioiTinh"),
                        L("DiaChi"),
                        L("UserName"),
                        L("AccessFailedCount"),
                        L("PhoneNumber"),
                        L("EmailAddress"),
                        L("EmailConfirmationCode"),
                        L("IsActive"),
                        L("IsEmailConfirmed"),
                        L("IsPhoneNumberConfirmed"),
                        L("PasswordResetCode"),
                        L("ProfilePicture"),
                        L("Password"),
                        L("Token"),
                        L("TokenExpire")
                        );

                    AddObjects(
                        sheet, 2, nguoiBenhs,
                        _ => _.NguoiBenh.HoVaTen,
                        _ => _.NguoiBenh.NgaySinh+"/"+_.NguoiBenh.ThangSinh+"/"+_.NguoiBenh.NamSinh,
                        _ => _.NguoiBenh.GioiTinh,
                        _ => _.NguoiBenh.DiaChi,
                        _ => _.NguoiBenh.UserName,
                        _ => _.NguoiBenh.AccessFailedCount,
                        _ => _.NguoiBenh.PhoneNumber,
                        _ => _.NguoiBenh.EmailAddress,
                        _ => _.NguoiBenh.EmailConfirmationCode,
                        _ => _.NguoiBenh.IsActive,
                        _ => _.NguoiBenh.IsEmailConfirmed,
                        _ => _.NguoiBenh.IsPhoneNumberConfirmed,
                        _ => _.NguoiBenh.PasswordResetCode,
                        _ => _.NguoiBenh.ProfilePicture,
                        _ => _.NguoiBenh.Password,
                        _ => _.NguoiBenh.Token,
                        _ => _timeZoneConverter.Convert(_.NguoiBenh.TokenExpire, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					
					for (var i = 1; i <= nguoiBenhs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[17], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(17);
                });
        }
    }
}
