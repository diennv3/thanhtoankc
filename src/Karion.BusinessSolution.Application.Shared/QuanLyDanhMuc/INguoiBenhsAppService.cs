using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface INguoiBenhsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNguoiBenhForViewDto>> GetAll(GetAllNguoiBenhsInput input);

		Task<GetNguoiBenhForEditOutput> GetNguoiBenhForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditNguoiBenhDto input);

		Task Delete(EntityDto input);
		Task<GetNguoiBenhForViewDto> GetNguoiBenhForView(int id);
        Task UpdateImageProfile(UpdateImageProfileInput input);
    }
}