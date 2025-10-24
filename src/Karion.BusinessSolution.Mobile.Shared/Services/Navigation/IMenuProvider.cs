using System.Collections.Generic;
using MvvmHelpers;
using Karion.BusinessSolution.Models.NavigationMenu;

namespace Karion.BusinessSolution.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}