using System.Collections.Generic;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.HanetTenant.Exporting
{
    public interface IHanetTenantLogsExcelExporter
    {
        FileDto ExportToFile(List<GetHanetTenantLogForViewDto> hanetTenantLogs);
    }
}