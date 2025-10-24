using Abp.Dependency;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Url;

namespace Karion.BusinessSolution.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor) :
            base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:WebSiteRootAddress";
    }
}