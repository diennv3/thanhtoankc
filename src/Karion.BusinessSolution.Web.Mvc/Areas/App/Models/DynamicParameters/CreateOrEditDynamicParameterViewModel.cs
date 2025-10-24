using System.Collections.Generic;
using Karion.BusinessSolution.DynamicEntityParameters.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.DynamicParameters
{
    public class CreateOrEditDynamicParameterViewModel
    {
        public DynamicParameterDto DynamicParameterDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
