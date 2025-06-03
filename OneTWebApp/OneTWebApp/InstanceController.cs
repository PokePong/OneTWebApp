using Microsoft.AspNetCore.Mvc;

namespace OneTWebApp;

[ApiController]
[Route("api/[controller]")]
public class InstanceController : Controller {
    [HttpGet]
    public IActionResult Index() {
        return Ok("Ca marche");
    }
}