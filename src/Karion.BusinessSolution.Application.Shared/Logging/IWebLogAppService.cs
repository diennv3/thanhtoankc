using Abp.Application.Services;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Logging.Dto;

namespace Karion.BusinessSolution.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
