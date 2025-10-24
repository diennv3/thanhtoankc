using System.Collections.Generic;
using Karion.BusinessSolution.Auditing.Dto;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
