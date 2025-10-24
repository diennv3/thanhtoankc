using System.Collections.Generic;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public interface IChuyenKhoasExcelExporter
    {
        FileDto ExportToFile(List<GetChuyenKhoaForViewDto> chuyenKhoas);
    }
}