using System.Threading.Tasks;
using Karion.BusinessSolution.Authorization.Users;

namespace Karion.BusinessSolution.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
