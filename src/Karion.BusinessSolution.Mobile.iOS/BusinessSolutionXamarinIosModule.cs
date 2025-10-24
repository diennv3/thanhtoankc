using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution
{
    [DependsOn(typeof(BusinessSolutionXamarinSharedModule))]
    public class BusinessSolutionXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionXamarinIosModule).GetAssembly());
        }
    }
}