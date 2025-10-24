using Karion.BusinessSolution.QuanLyDanhMuc;

namespace Karion.BusinessSolution.MobileAppServices.MobileAppServices.JwtBearerMobile
{
    public interface IJwtMobileUtils
    {
        string GenerateToken(NguoiBenh nguoiBenh);
        int? ValidateToken(string token);
    }
}
