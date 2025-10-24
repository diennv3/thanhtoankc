using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karion.BusinessSolution.Authorization.Permissions.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Common.Modals
{
    public class PermissionTreeModalViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
