using Abp.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Karion.BusinessSolution.Authorization.Roles;

namespace Karion.BusinessSolution.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
        }
    }
}
