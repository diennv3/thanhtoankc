using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Permissions.Dto;

namespace Karion.BusinessSolution.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}