using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class NguoiThansExcelExporter : NpoiExcelExporterBase, INguoiThansExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NguoiThansExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNguoiThanForViewDto> nguoiThans)
        {
            return CreateExcelPackage(
                "NguoiThans.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("NguoiThans"));

                    AddHeader(
                        sheet,
                        L("HoVaTen"),
                        L("Tuoi"),
                        L("GioiTinh"),
                        L("DiaChi"),
                        L("MoiQuanHe"),
                        L("SoDienThoai"),
                        (L("NguoiBenh")) + L("HoVaTen")
                        );

                    AddObjects(
                        sheet, 2, nguoiThans,
                        _ => _.NguoiThan.HoVaTen,
                        _ => _.NguoiThan.Tuoi,
                        _ => _.NguoiThan.GioiTinh,
                        _ => _.NguoiThan.DiaChi,
                        _ => _.NguoiThan.MoiQuanHe,
                        _ => _.NguoiThan.SoDienThoai,
                        _ => _.NguoiBenhHoVaTen
                        );

					
					
                });
        }
    }
}
