using API.Attributes;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService service): ControllerBase
{
    [HttpPost]
    [Route("signup")]
    public ActionResult<UserResponseDTO> PSignup([FromBody] UserSignupRequestDTO request)
    {
        return Ok(service.Signup(request));
    }
    
    
    [HttpPost]
    [Route("login")]
    public ActionResult<UserResponseDTO> PLogin([FromBody] UserLoginRequestDTO data)
    {
        return Ok(service.Login(data));
    }
    
    
    [HttpGet]
    [Route("@me")]
    [Authenticated]
    public ActionResult<AuthorizedUserResponseDTO> GGetUser()
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(authUser);
    }
}