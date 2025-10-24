using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Karion.BusinessSolution.DataExporting.Excel.NPOI;
using Karion.BusinessSolution.NguoiBenhTestNS.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.NguoiBenhTestNS.Exporting
{
    public class NguoiBenhTestsExcelExporter : NpoiExcelExporterBase, INguoiBenhTestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NguoiBenhTestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNguoiBenhTestForViewDto> nguoiBenhTests)
        {
            return CreateExcelPackage(
                "NguoiBenhTests.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("NguoiBenhTests"));

                    AddHeader(
                        sheet,
                        L("Ten")
                        );

                    AddObjects(
                        sheet, 2, nguoiBenhTests,
                        _ => _.NguoiBenhTest.Ten
                        );

					
					
                });
        }
    }
}
