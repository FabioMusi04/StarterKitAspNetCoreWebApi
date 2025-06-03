using Microsoft.AspNetCore.Mvc;

namespace StarterKit.Controllers
{
    [ApiController]
    public class HomeController() : ControllerBase
    {
        [Route("/")]
        [HttpGet]
        public IActionResult Home()
        {
            return Ok(new { message = "Welcome to the Starter Kit API!" });
        }
    }
}