using System.Linq;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Karion.BusinessSolution.EntityFrameworkCore;
using Karion.BusinessSolution.EntityFrameworkCore.Repositories;

namespace Karion.BusinessSolution.MultiTenancy.Payments
{
    public class SubscriptionPaymentRepository : BusinessSolutionRepositoryBase<SubscriptionPayment, long>, ISubscriptionPaymentRepository
    {
        public SubscriptionPaymentRepository(IDbContextProvider<BusinessSolutionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return await SingleAsync(p => p.ExternalPaymentId == paymentId && p.Gateway == gateway);
        }

        public async Task<SubscriptionPayment> GetLastCompletedPaymentOrDefaultAsync(int tenantId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring)
        {
            return (await GetAll()
                    .Where(p => p.TenantId == tenantId)
                    .Where(p => p.Status == SubscriptionPaymentStatus.Completed)
                    .WhereIf(gateway.HasValue, p => p.Gateway == gateway.Value)
                    .WhereIf(isRecurring.HasValue, p => p.IsRecurring == isRecurring.Value)
                    .ToListAsync()
                )
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }

        public async Task<SubscriptionPayment> GetLastPaymentOrDefaultAsync(int tenantId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring)
        {
            return (await GetAll()
                    .Where(p => p.TenantId == tenantId)
                    .WhereIf(gateway.HasValue, p => p.Gateway == gateway.Value)
                    .WhereIf(isRecurring.HasValue, p => p.IsRecurring == isRecurring.Value)
                    .ToListAsync()).OrderByDescending(x => x.Id
                )
                .FirstOrDefault();
        }
    }
}
