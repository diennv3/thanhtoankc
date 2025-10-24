using System.Threading.Tasks;
using Karion.BusinessSolution.Sessions.Dto;

namespace Karion.BusinessSolution.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
