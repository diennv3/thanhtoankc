using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Karion.BusinessSolution.MobileAppServices.MobileAppServices.JwtBearerMobile
{
    public class BussinessSolutionMobileJwtTokenUtils : IJwtMobileUtils
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly string _securityKey;

        public BussinessSolutionMobileJwtTokenUtils(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = _env.GetAppConfiguration();
            _securityKey = _appConfiguration["Authentication:JwtBearer:IsEnabled"];
        }

        public string GenerateToken(NguoiBenh nguoiBenh)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", nguoiBenh.Id.ToString()), 
                    //new Claim("UserName", nguoiBenh.UserName), 
                    //new Claim("HoVaTen", nguoiBenh.HoVaTen) 
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securityKey);

            try
            {
                tokenHandler.ValidateToken(
                    token, 
                    new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, 
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var nguoiBenhId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return nguoiBenhId;
            }
            catch
            {
                return null;
            }
        }

    }
}
