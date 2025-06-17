namespace RefreshToken.Api.Interfaces
{
    public interface ICookieService
    {
        void AppendCookie(HttpResponse response, string value);

        void RemoveCookie(HttpRequest request, HttpResponse response);
    }

}