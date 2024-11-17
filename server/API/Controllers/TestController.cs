using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("auth")]
    [Authenticated]
    public ActionResult<Boolean> GText([FromQuery] string example)
    {
        return Ok(true);
    }
}