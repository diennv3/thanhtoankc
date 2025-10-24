using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution
{
    public class BusinessSolutionClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionClientModule).GetAssembly());
        }
    }
}
