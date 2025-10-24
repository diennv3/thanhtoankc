using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution
{
    [DependsOn(typeof(BusinessSolutionCoreSharedModule))]
    public class BusinessSolutionApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionApplicationSharedModule).GetAssembly());
        }
    }
}