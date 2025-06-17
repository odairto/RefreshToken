using RefreshToken.Api.Interfaces;

namespace RefreshToken.Api.Services
{
    public class CookieService : ICookieService
    {
        private readonly int seconds = 5;
        private readonly IConfiguration _configuration;

        public CookieService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AppendCookies(HttpResponse response, string value, string userName)
        {
            response.Cookies.Append(_configuration["CookieName"] ?? "token", value, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                MaxAge = TimeSpan.FromSeconds(seconds)
            });

            string refreshToken = Guid.NewGuid().ToString();
            response.Cookies.Append(_configuration["RefreshCookieName"] ?? "token",
                refreshToken, 
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                    MaxAge = TimeSpan.FromDays(1)
                });

            RefreshTokenStore.Tokens[userName] = refreshToken;
        }

        public void RemoveCookie(HttpRequest request, HttpResponse response)
        {
            if (request.Cookies[_configuration["CookieName"] ?? "token"] != null)
            {
                response.Cookies.Delete(_configuration["CookieName"] ?? "token");
            }
        }
    }
}
