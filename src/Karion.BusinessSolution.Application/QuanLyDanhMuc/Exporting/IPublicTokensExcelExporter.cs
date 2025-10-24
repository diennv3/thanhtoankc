using System.Collections.Generic;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Exporting
{
    public interface IPublicTokensExcelExporter
    {
        FileDto ExportToFile(List<GetPublicTokenForViewDto> publicTokens);
    }
}