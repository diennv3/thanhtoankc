using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.HanetTenant
{
    public interface IHanetFaceDetectedsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHanetFaceDetectedForViewDto>> GetAll(GetAllHanetFaceDetectedsInput input);

        Task<GetHanetFaceDetectedForViewDto> GetHanetFaceDetectedForView(long id);

		Task<GetHanetFaceDetectedForEditOutput> GetHanetFaceDetectedForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHanetFaceDetectedDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetHanetFaceDetectedsToExcel(GetAllHanetFaceDetectedsForExcelInput input);

		
    }
}