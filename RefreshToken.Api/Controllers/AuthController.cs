using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RefreshToken.Api.Entities;
using RefreshToken.Api.Interfaces;
using RefreshToken.Api.Services;

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
                _cookieService.AppendCookie(Response, jwtToken);
                return Ok();

            }
            return Unauthorized();
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
