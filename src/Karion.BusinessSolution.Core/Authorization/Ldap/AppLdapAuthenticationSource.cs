using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.MultiTenancy;

namespace Karion.BusinessSolution.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}