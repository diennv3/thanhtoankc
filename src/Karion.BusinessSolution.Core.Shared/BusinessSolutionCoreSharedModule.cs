using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution
{
    public class BusinessSolutionCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionCoreSharedModule).GetAssembly());
        }
    }
}