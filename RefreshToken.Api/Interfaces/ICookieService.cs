namespace RefreshToken.Api.Interfaces
{
    public interface ICookieService
    {
        void AppendCookies(HttpResponse response, string value, string userName);

        void RemoveCookie(HttpRequest request, HttpResponse response);
    }

}