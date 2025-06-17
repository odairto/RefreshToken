using System.Security.Claims;
using RefreshToken.Api.Entities;

namespace RefreshToken.Api.Interfaces
{

    public interface IAuthenticationService
    {
        string GenerateJwtToken(User user);

        string? GetJwtFromCookie(HttpRequest request);
        User LoginUser(User login);
        bool ValidateJwtToken(string token);
        bool ValidateJwtToken(string token, out ClaimsPrincipal? claimsPrincipal);

    }
}
