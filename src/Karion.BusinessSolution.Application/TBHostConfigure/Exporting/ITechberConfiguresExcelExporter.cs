using System.Collections.Generic;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.TBHostConfigure.Exporting
{
    public interface ITechberConfiguresExcelExporter
    {
        FileDto ExportToFile(List<GetTechberConfigureForViewDto> techberConfigures);
    }
}