using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    /*[HttpGet]
    [Route("auth")]
    [Authenticated]
    public ActionResult<Boolean> ExampleAuthenticated()
    {
        return Ok(true);
    }
    
    
    [HttpGet]
    [Route("admin")]
    [Rolepolicy("Admin")]
    public ActionResult<Boolean> ExampleAuthenticatedWithRole()
    {
        return Ok(true);
    }*/
}