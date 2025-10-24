using System.Collections.Generic;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public interface IBacSiChuyenKhoasExcelExporter
    {
        FileDto ExportToFile(List<GetBacSiChuyenKhoaForViewDto> bacSiChuyenKhoas);
    }
}