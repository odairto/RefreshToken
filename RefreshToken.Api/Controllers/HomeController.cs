using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RefreshToken.Api.Controllers
{

    // Protected route, call /api/auth/login as post with user/pass: "admin" / "admin"
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult InternalResource()
        {
            var response = new { Name = "John" };
            return Ok(response);
        }
    }
}
