using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Karion.BusinessSolution
{
    [DependsOn(typeof(BusinessSolutionClientModule), typeof(AbpAutoMapperModule))]
    public class BusinessSolutionXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BusinessSolutionXamarinSharedModule).GetAssembly());
        }
    }
}