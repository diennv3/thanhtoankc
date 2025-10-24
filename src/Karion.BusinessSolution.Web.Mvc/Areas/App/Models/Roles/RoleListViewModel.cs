using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization.Permissions.Dto;
using Karion.BusinessSolution.Web.Areas.App.Models.Common;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}