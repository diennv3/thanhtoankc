using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Karion.BusinessSolution.Authorization;

namespace Karion.BusinessSolution
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(BusinessSolutionApplicationSharedModule),
        typeof(BusinessSolutionCoreModule)
        )]
    public class BusinessSolutionApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionApplicationModule).GetAssembly());
        }
    }
}