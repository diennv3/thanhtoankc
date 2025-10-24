using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Web;

namespace Karion.BusinessSolution.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class BusinessSolutionDbContextFactory : IDesignTimeDbContextFactory<BusinessSolutionDbContext>
    {
        public BusinessSolutionDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BusinessSolutionDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            BusinessSolutionDbContextConfigurer.Configure(builder, configuration.GetConnectionString(BusinessSolutionConsts.ConnectionStringName));

            return new BusinessSolutionDbContext(builder.Options);
        }
    }
}