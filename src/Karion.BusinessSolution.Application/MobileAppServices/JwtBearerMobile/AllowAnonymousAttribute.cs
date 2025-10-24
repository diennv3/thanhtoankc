using System;

namespace Karion.BusinessSolution.MobileAppServices.MobileAppServices.JwtBearerMobile
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
