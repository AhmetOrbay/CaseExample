using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HotelLibrary.Extensions
{
    public  class JwtHandler
    {
        private readonly string _secretKey;

        public JwtHandler(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GenerateJwt(string key)
        {
            var claims = new[]
            {
            new Claim("JwtSecurityKey2", key)
        };
            //burada secret key alacagiz.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public bool ValidateJwt(string jwt, string expectedKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwt, validationParameters, out _);
                var keyClaim = claimsPrincipal.FindFirst("JwtSecurityKey2")?.Value;

                if (keyClaim == expectedKey)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
