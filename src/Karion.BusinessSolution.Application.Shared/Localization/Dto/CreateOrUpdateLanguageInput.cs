using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}