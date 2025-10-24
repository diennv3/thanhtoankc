using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution
{
    [DependsOn(typeof(BusinessSolutionXamarinSharedModule))]
    public class BusinessSolutionXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionXamarinAndroidModule).GetAssembly());
        }
    }
}