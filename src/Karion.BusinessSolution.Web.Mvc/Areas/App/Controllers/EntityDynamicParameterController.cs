using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.DynamicEntityParameters;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.DynamicEntityParameters;
using Karion.BusinessSolution.Web.Areas.App.Models.EntityDynamicParameters;
using Karion.BusinessSolution.Web.Controllers;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameters)]
    public class EntityDynamicParameterController : BusinessSolutionControllerBase
    {
        private readonly IDynamicParameterAppService _dynamicParameterAppService;
        private readonly IDynamicEntityParameterDefinitionManager _dynamicEntityParameterDefinitionManager;

        public EntityDynamicParameterController(
            IDynamicParameterAppService dynamicParameterAppService,
            IDynamicEntityParameterDefinitionManager dynamicEntityParameterDefinitionManager
            )
        {
            _dynamicParameterAppService = dynamicParameterAppService;
            _dynamicEntityParameterDefinitionManager = dynamicEntityParameterDefinitionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameters_Create)]
        public async Task<IActionResult> CreateModal()
        {
            var model = new CreateEntityDynamicParameterViewModel()
            {
                DynamicParameters = (await _dynamicParameterAppService.GetAll()).Items.ToList(),
                AllEntities = _dynamicEntityParameterDefinitionManager.GetAllEntities()
            };

            return PartialView("_CreateModal", model);
        }
    }
}