using System;
using Karion.BusinessSolution.Core;
using Karion.BusinessSolution.Core.Dependency;
using Karion.BusinessSolution.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karion.BusinessSolution.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}