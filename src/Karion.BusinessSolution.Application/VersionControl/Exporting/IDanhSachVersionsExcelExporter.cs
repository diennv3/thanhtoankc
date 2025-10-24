using System.Collections.Generic;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.VersionControl.Exporting
{
    public interface IDanhSachVersionsExcelExporter
    {
        FileDto ExportToFile(List<GetDanhSachVersionForViewDto> danhSachVersions);
    }
}