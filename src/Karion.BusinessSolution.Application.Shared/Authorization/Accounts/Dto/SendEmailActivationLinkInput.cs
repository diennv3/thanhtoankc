using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}