using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Permissions.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}