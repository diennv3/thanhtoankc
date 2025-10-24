using System.Collections.Generic;
using Karion.BusinessSolution.NguoiBenhTestNS.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.NguoiBenhTestNS.Exporting
{
    public interface INguoiBenhTestsExcelExporter
    {
        FileDto ExportToFile(List<GetNguoiBenhTestForViewDto> nguoiBenhTests);
    }
}