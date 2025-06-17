using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RefreshToken.Api.Entities;
using RefreshToken.Api.Interfaces;

namespace RefreshToken.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICookieService _cookieService;

        public AuthController(IAuthenticationService authenticationService, ICookieService cookieService)
        {
            _authenticationService = authenticationService;
            _cookieService = cookieService;
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] User login)
        {
            User user = _authenticationService.LoginUser(login);

            if (user != null)
            {
                var jwtToken = _authenticationService.GenerateJwtToken(user);
                _cookieService.AppendCookies(Response, jwtToken, login.UserName);
                return Ok();

            }
            return Unauthorized();
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Refresh token não encontrado.");

            var userId = _refreshTokenService.GetUserIdByToken(refreshToken); // Supõe que você tem um método assim

            if (userId == null)
                return Unauthorized("Refresh token inválido.");

            //var user = _userService.GetById(userId); // Você pode ajustar conforme seu serviço

            //if (user == null)
            //    return Unauthorized("Usuário não encontrado.");

            // Novo JWT
            var jwtToken = _authenticationService.GenerateJwtToken(user);
            _cookieService.AppendCookie(Response, jwtToken);

            // (Opcional) Gerar novo refresh token
            var newRefreshToken = _refreshTokenService.GenerateAndStore(user.Id);
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok("Token renovado com sucesso.");
        }

        //// GET: AuthController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AuthController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AuthController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AuthController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
