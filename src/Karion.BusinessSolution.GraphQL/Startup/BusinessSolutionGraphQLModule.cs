using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution.Startup
{
    [DependsOn(typeof(BusinessSolutionCoreModule))]
    public class BusinessSolutionGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}