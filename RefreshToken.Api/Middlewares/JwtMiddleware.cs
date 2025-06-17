using System.Security.Claims;
using RefreshToken.Api.Interfaces;

namespace RefreshToken.Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string CookieName;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        CookieName = configuration["CookieName"] ?? "token";
    }

    public async Task InvokeAsync(HttpContext context, IAuthenticationService authenticationService)
    {
        if (context.Request.Path.StartsWithSegments("/api/auth/login"))
        {
            await _next(context);
            return;
        }

        if (context.Request.Cookies.ContainsKey(CookieName))
        {
            var token = context.Request.Cookies[CookieName];

            if (token == null)
                return;

            if (authenticationService.ValidateJwtToken(token, out ClaimsPrincipal? claimsPrincipal))
            {
                if (claimsPrincipal != null)
                {
                    context.User = claimsPrincipal;
                    var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        //Adding the User Id in the context, its possible to use it to validade permissions
                        context.Items["Id"] = userId;
                    }
                }
            }
        }

        await _next(context);
    }
}