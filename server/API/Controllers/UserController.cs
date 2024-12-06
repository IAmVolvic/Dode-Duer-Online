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
    [Authenticated]
    public ActionResult<AuthorizedUserResponseDTO> PEnroll([FromBody] UserEnrollmentRequestDTO data)
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(service.EnrollUser(authUser.Id, data));
    }
    
    
    [HttpGet]
    [Route("@user")]
    [Authenticated]
    public ActionResult<AuthorizedUserResponseDTO> GGetUser()
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(authUser);
    }
    
    [HttpPatch]
    [Route("@user/update")]
    [Authenticated]
    public ActionResult<AuthorizedUserResponseDTO> PUpdateUser([FromBody] UserUpdateRequestDTO data)
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(service.UpdateUser(authUser.Id, data));
    }
}