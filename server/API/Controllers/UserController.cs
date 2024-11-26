using API.Attributes;
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
    [Route("@admin/signup")]
    [Rolepolicy("Admin")]
    public ActionResult<AuthorizedUserResponseDTO> PSignup([FromBody] UserSignupRequestDTO request)
    {
        return Ok(service.Signup(request));
    }
    
    
    [HttpPost]
    [Route("@user/login")]
    public ActionResult<UserResponseDTO> PLogin([FromBody] UserLoginRequestDTO data)
    {
        return Ok(service.Login(data));
    }
    
    
    [HttpPatch]
    [Route("@user/enroll")]
    public ActionResult<AuthorizedUserResponseDTO> PEnroll([FromBody] UserEnrollmentRequestDTO data)
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(service.EnrollUser(authUser.Id, data));
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