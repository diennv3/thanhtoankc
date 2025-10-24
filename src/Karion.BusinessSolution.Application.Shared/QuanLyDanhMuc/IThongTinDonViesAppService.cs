using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.TBHostConfigure.Dtos;


namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IThongTinDonViesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetThongTinDonViForViewDto>> GetAll(GetAllThongTinDonViesInput input);

        Task<GetThongTinDonViForViewDto> GetThongTinDonViForView(int id);

		Task<GetThongTinDonViForEditOutput> GetThongTinDonViForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditThongTinDonViDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetThongTinDonViesToExcel(GetAllThongTinDonViesForExcelInput input);
		Task<List<BankCodeDto>> GetListBankCode();
		Task<GetTenantBankInfoDto> GetTenantBankInfo();

    }
}