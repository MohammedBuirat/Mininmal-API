using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinimalAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("/hello")]
        public ActionResult print()
        {
            return Ok("hi");
        }
    }
}
