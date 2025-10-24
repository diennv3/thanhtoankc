

using Karion.BusinessSolution.QuanLyDanhMuc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Karion.BusinessSolution.MobileAppServices.MobileAppServices.JwtBearerMobile
{
    public class JwtMobileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly INguoiBenhsAppService _nguoiBenhsAppService;
        //private readonly AppSettings _appSettings;

        public JwtMobileMiddleware(RequestDelegate next, INguoiBenhsAppService nguoiBenhsAppService)
        {
            _next = next;
            _nguoiBenhsAppService = nguoiBenhsAppService;
        }

        public async Task Invoke(HttpContext context, IJwtMobileUtils jwtMobileUtils) 
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var nguoiBenhId = jwtMobileUtils.ValidateToken(token);
            if(nguoiBenhId != null)
            {
                context.Items["NguoiBenh"] = _nguoiBenhsAppService.GetNguoiBenhForView((int)nguoiBenhId);
            }
            await _next(context);
        }

    }
}
