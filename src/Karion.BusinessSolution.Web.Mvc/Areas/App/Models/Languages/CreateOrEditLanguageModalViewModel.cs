using Abp.AutoMapper;
using Karion.BusinessSolution.Localization.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}