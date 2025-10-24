using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization.Roles.Dto;

namespace Karion.BusinessSolution.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    public interface IRoleAppService : IApplicationService
    {
        Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input);

        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);

        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);

        Task DeleteRole(EntityDto input);
    }
}