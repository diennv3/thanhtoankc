using System.Collections.Generic;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public interface IGiaDichVusExcelExporter
    {
        FileDto ExportToFile(List<GetGiaDichVuForViewDto> giaDichVus);
    }
}