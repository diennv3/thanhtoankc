using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization.Permissions.Dto;

namespace Karion.BusinessSolution.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
