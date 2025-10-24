using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Karion.BusinessSolution.EntityFrameworkCore;

namespace Karion.BusinessSolution.HealthChecks
{
    public class BusinessSolutionDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public BusinessSolutionDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("BusinessSolutionDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("BusinessSolutionDbContext could not connect to database"));
        }
    }
}
