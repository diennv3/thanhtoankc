using Abp.AspNetCore.Mvc.Authorization;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}