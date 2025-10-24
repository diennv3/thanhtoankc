using System.Threading.Tasks;

namespace Karion.BusinessSolution.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}