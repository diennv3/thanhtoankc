using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Permissions.Dto;

namespace Karion.BusinessSolution.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}