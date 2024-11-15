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
        try
        {
            var newUser = service.CreateNewUser(data);
            return Ok(newUser);
        } catch(Exception e) {
            return BadRequest( "Request Failed: " + e.Message );
        }
    }
    
    
    [HttpPost]
    [Route("login")]
    public ActionResult<string> ExampleLogin([FromBody] string data)
    {
        service.Login(data);
        return Ok("Bob");
    }
}