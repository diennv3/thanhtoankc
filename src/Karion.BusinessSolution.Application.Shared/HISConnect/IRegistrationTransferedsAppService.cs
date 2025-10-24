using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.HISConnect.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.HISConnect
{
    public interface IRegistrationTransferedsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRegistrationTransferedForViewDto>> GetAll(GetAllRegistrationTransferedsInput input);

		Task<GetRegistrationTransferedForEditOutput> GetRegistrationTransferedForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRegistrationTransferedDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<RegistrationTransferedLichHenKhamLookupTableDto>> GetAllLichHenKhamForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RegistrationTransferedNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		
    }
}