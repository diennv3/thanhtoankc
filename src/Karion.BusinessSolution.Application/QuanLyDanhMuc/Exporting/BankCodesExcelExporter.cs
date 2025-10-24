using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public class BankCodesExcelExporter : NpoiExcelExporterBase, IBankCodesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BankCodesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankCodeForViewDto> bankCodes)
        {
            return CreateExcelPackage(
                "BankCodes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("BankCodes"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("BankName")
                        );

                    AddObjects(
                        sheet, 2, bankCodes,
                        _ => _.BankCode.Code,
                        _ => _.BankCode.BankName
                        );

					
					
                });
        }
    }
}
