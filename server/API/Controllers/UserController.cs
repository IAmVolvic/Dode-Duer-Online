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
        string exampleVar = Environment.GetEnvironmentVariable("Example");
        return "Worked: " + exampleVar;
    }
    
}