using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
