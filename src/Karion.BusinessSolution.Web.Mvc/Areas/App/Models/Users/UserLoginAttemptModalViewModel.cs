using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Users.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}