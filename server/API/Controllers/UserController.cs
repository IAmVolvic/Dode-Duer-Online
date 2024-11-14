using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController
{
    [HttpGet]
    public ActionResult<string> ExampleCode()
    {
        return "Worked:";
    }
    
}