using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.PortalAppServices
{
    public interface IPortalAppServices : IApplicationService
    {
        Task<PortalDto.PortalGetListChuyenKhoaResult> PortalGetListChuyenKhoa(PortalDto.PortalGetListChuyenKhoaInput model);
        Task<MobileDto.CommonResponse> PortalRegister(PortalDto.PortalRegisterInput input);
    }
}