using Abp.AutoMapper;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Authorization.Users.Dto;
using Karion.BusinessSolution.Web.Areas.App.Models.Common;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}