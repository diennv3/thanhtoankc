using System.Collections.Generic;
using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Exporting
{
    public interface IAttendancesExcelExporter
    {
        FileDto ExportToFile(List<GetAttendanceForViewDto> attendances);
    }
}