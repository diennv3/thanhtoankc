using System.Collections.Generic;
using Abp;
using Karion.BusinessSolution.Chat.Dto;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
