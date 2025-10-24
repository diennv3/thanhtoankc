using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface INguoiBenhNotificationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNguoiBenhNotificationForViewDto>> GetAll(GetAllNguoiBenhNotificationsInput input);

        Task<GetNguoiBenhNotificationForViewDto> GetNguoiBenhNotificationForView(long id);

		Task<GetNguoiBenhNotificationForEditOutput> GetNguoiBenhNotificationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditNguoiBenhNotificationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetNguoiBenhNotificationsToExcel(GetAllNguoiBenhNotificationsForExcelInput input);

		
		Task<PagedResultDto<NguoiBenhNotificationNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input);
		
    }
}