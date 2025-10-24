using System.Collections.Generic;
using Karion.BusinessSolution.Caching.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}