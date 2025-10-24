using Karion.BusinessSolution.QuanLyDanhMuc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Karion.BusinessSolution.MobileAppServices.MobileAppServices.JwtBearerMobile
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MobileAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var nguoiBenh = (NguoiBenh)context.HttpContext.Items["NguoiBenh"];
            if(nguoiBenh == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
