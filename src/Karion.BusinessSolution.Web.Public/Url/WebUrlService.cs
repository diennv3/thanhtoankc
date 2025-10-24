using Abp.Dependency;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Url;
using Karion.BusinessSolution.Web.Url;

namespace Karion.BusinessSolution.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}