using Microsoft.Extensions.Configuration;

namespace Karion.BusinessSolution.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
