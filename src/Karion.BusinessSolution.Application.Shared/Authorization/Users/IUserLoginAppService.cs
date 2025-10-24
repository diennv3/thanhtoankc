using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization.Users.Dto;

namespace Karion.BusinessSolution.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
