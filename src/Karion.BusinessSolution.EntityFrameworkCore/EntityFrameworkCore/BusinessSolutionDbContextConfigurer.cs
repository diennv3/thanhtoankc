using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.EntityFrameworkCore
{
    public static class BusinessSolutionDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BusinessSolutionDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BusinessSolutionDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}