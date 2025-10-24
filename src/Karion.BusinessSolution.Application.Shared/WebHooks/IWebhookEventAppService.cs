using System.Threading.Tasks;
using Abp.Webhooks;

namespace Karion.BusinessSolution.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
