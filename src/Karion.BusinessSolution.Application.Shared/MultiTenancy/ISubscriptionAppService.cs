using System.Threading.Tasks;
using Abp.Application.Services;

namespace Karion.BusinessSolution.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
