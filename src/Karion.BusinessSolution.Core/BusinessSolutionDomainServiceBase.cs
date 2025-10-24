using Abp.Domain.Services;

namespace Karion.BusinessSolution
{
    public abstract class BusinessSolutionDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected BusinessSolutionDomainServiceBase()
        {
            LocalizationSourceName = BusinessSolutionConsts.LocalizationSourceName;
        }
    }
}
