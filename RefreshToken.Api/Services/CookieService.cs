using RefreshToken.Api.Interfaces;

namespace RefreshToken.Api.Services
{
    public class CookieService : ICookieService
    {
        private readonly int days = 30;
        private readonly IConfiguration _configuration;

        public CookieService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AppendCookie(HttpResponse response, string value)
        {
            response.Cookies.Append(_configuration["CookieName"] ?? "token", value, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                MaxAge = TimeSpan.FromDays(days)
            });
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
