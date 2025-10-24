using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IThongTinBacSiMoRongsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetThongTinBacSiMoRongForViewDto>> GetAll(GetAllThongTinBacSiMoRongsInput input);

        Task<GetThongTinBacSiMoRongForViewDto> GetThongTinBacSiMoRongForView(int id);

		Task<GetThongTinBacSiMoRongForEditOutput> GetThongTinBacSiMoRongForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditThongTinBacSiMoRongDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetThongTinBacSiMoRongsToExcel(GetAllThongTinBacSiMoRongsForExcelInput input);

		
		Task<PagedResultDto<ThongTinBacSiMoRongUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}