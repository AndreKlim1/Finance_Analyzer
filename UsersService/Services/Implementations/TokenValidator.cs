using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsersService.Services.Implementations
{
    public static class TokenValidator
    {
        public static ClaimsPrincipal? ValidateToken(string token, IConfiguration config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["JWT:Secret"]);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JWT:ValidIssuer"],

                    ValidateAudience = true,
                    ValidAudience = config["JWT:ValidAudience"],

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);

                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
    }
}
