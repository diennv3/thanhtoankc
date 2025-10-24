using Abp.AutoMapper;
using Karion.BusinessSolution.Authorization.Roles.Dto;
using Karion.BusinessSolution.Web.Areas.App.Models.Common;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}