using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class ThongTinBacSiMoRongsExcelExporter : NpoiExcelExporterBase, IThongTinBacSiMoRongsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ThongTinBacSiMoRongsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetThongTinBacSiMoRongForViewDto> thongTinBacSiMoRongs)
        {
            return CreateExcelPackage(
                "ThongTinBacSiMoRongs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ThongTinBacSiMoRongs"));

                    AddHeader(
                        sheet,
                        L("Image"),
                        L("TieuSu"),
                        L("ChucDanh"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, thongTinBacSiMoRongs,
                        _ => _.ThongTinBacSiMoRong.Image,
                        _ => _.ThongTinBacSiMoRong.TieuSu,
                        _ => _.ThongTinBacSiMoRong.ChucDanh,
                        _ => _.UserName
                        );

					
					
                });
        }
    }
}
