using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Webhooks;
using Karion.BusinessSolution.Authorization;

namespace Karion.BusinessSolution.WebHooks
{
    [AbpAuthorize(AppPermissions.Pages_Administration_WebhookSubscription)]
    public class WebhookEventAppService : BusinessSolutionAppServiceBase, IWebhookEventAppService
    {
        private readonly IWebhookEventStore _webhookEventStore;

        public WebhookEventAppService(IWebhookEventStore webhookEventStore)
        {
            _webhookEventStore = webhookEventStore;
        }

        public async Task<WebhookEvent> Get(string id)
        {
            return await _webhookEventStore.GetAsync(AbpSession.TenantId, Guid.Parse(id));
        }
    }
}
