using System.Threading.Tasks;

namespace Karion.BusinessSolution.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}