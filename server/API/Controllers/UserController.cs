using DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService service): ControllerBase
{
    [HttpPost]
    [Route("signup")]
    public ActionResult<User> Signup([FromBody] UserSignupDTO data)
    {
        var cool = service.CreateNewUser(data);
        return Ok("Worked "+ cool.Passwordhash);
    }
    
    
    [HttpPost]
    [Route("login")]
    public ActionResult<string> ExampleLogin()
    {
        return "Worked:";
    }
    
}