using Microsoft.Extensions.DependencyInjection;
using Karion.BusinessSolution.HealthChecks;

namespace Karion.BusinessSolution.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<BusinessSolutionDbContextHealthCheck>("Database Connection");
            builder.AddCheck<BusinessSolutionDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
