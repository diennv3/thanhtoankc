using Abp.Authorization;
using Karion.BusinessSolution.Authorization.Roles;
using Karion.BusinessSolution.Authorization.Users;

namespace Karion.BusinessSolution.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
