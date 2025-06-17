using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RefreshToken.Api.Entities;
using RefreshToken.Api.Interfaces;

namespace RefreshToken.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = configuration["JwtSettings:Key"] ?? string.Empty;
            _issuer = configuration["JwtSettings:Issuer"] ?? string.Empty;
            _audience = configuration["JwtSettings:Audience"] ?? string.Empty;
        }

        public string GenerateJwtToken(User user)
        {
            if (user is null || user.Id == 0 || string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentNullException(nameof(user));
            }
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _issuer),
            new Claim("Id", user.Id.ToString()),
            new Claim("u", user.UserName.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(3000),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? GetJwtFromCookie(HttpRequest request)
        {
            return request.Cookies[_configuration["CookieName"] ?? "omf_token"];
        }

        public User LoginUser(User login)
        {
            //Change for real validation of user

            User user = new User
            {
                Id = 1,
                UserName = "admin",
                Password = "admin"
            };



            if (login.UserName == user.UserName && login.Password == user.Password)
            {

                return user;
            }

            return null;
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateJwtToken(string token, out ClaimsPrincipal? claimsPrincipal)
        {
            claimsPrincipal = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty);

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
