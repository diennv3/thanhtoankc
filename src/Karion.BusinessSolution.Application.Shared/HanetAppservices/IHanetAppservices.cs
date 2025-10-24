using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Newtonsoft.Json.Linq;

namespace Karion.BusinessSolution.MobileAppServices
{
    public interface IHanetAppservices : IApplicationService
    {
        Task HanetWebhookGetAuthorizationCode(string code);
        Task<MobileDto.CommonResponse> HanetWebhookGetAccessToken();
        Task HanetWebhookAddPlace(HanetTenantPlaceDatasDto input);
        Task UpdateDevices(int placeid);
        Task updateDevicesStatus();
        Task Webhook(JObject input);
        Task RegisterUser(NguoiBenhDto input, bool isEditMode);
    }
}