using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Delegation;
using Karion.BusinessSolution.Authorization.Users.Delegation.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
