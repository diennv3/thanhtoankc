using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Sessions.Dto;

namespace Karion.BusinessSolution.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
