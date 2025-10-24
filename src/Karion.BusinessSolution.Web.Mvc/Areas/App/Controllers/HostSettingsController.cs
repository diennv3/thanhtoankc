using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Configuration.Host;
using Karion.BusinessSolution.Editions;
using Karion.BusinessSolution.Timing;
using Karion.BusinessSolution.Timing.Dto;
using Karion.BusinessSolution.Web.Areas.App.Models.HostSettings;
using Karion.BusinessSolution.Web.Controllers;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsController : BusinessSolutionControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IHostSettingsAppService _hostSettingsAppService;
        private readonly IEditionAppService _editionAppService;
        private readonly ITimingAppService _timingAppService;

        public HostSettingsController(
            IHostSettingsAppService hostSettingsAppService,
            UserManager userManager, 
            IEditionAppService editionAppService, 
            ITimingAppService timingAppService)
        {
            _hostSettingsAppService = hostSettingsAppService;
            _userManager = userManager;
            _editionAppService = editionAppService;
            _timingAppService = timingAppService;
        }

        public async Task<ActionResult> Index()
        {
            var hostSettings = await _hostSettingsAppService.GetAllSettings();
            var editionItems = await _editionAppService.GetEditionComboboxItems(hostSettings.TenantManagement.DefaultEditionId);
            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.Application,
                SelectedTimezoneId = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone)
            });

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            ViewBag.CurrentUserEmail = user.EmailAddress;

            var model = new HostSettingsViewModel
            {
                Settings = hostSettings,
                EditionItems = editionItems,
                TimezoneItems = timezoneItems
            };

            return View(model);
        }
    }
}