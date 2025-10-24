using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.DynamicEntityParameters.Dto;
using Karion.BusinessSolution.EntityDynamicParameterValues.Dto;

namespace Karion.BusinessSolution.DynamicEntityParameters
{
    public interface IEntityDynamicParameterValueAppService
    {
        Task<EntityDynamicParameterValueDto> Get(int id);

        Task<ListResultDto<EntityDynamicParameterValueDto>> GetAll(GetAllInput input);

        Task Add(EntityDynamicParameterValueDto input);

        Task Update(EntityDynamicParameterValueDto input);

        Task Delete(int id);

        Task<GetAllEntityDynamicParameterValuesOutput> GetAllEntityDynamicParameterValues(GetAllEntityDynamicParameterValuesInput input);
    }
}
