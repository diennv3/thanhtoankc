using System.Collections.Generic;
using Karion.BusinessSolution.Editions.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}