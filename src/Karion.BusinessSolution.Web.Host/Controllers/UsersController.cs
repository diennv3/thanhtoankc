using Abp.AspNetCore.Mvc.Authorization;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.Storage;
using Abp.BackgroundJobs;

namespace Karion.BusinessSolution.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}