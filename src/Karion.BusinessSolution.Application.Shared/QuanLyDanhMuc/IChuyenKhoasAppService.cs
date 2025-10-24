using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IChuyenKhoasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChuyenKhoaForViewDto>> GetAll(GetAllChuyenKhoasInput input);

        Task<GetChuyenKhoaForViewDto> GetChuyenKhoaForView(int id);

		Task<GetChuyenKhoaForEditOutput> GetChuyenKhoaForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditChuyenKhoaDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetChuyenKhoasToExcel(GetAllChuyenKhoasForExcelInput input);

		
    }
}