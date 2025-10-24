using Abp.Application.Services.Dto;
using Abp.Webhooks;
using Karion.BusinessSolution.WebHooks.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
