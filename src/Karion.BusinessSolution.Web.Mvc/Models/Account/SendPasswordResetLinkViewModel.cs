using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}