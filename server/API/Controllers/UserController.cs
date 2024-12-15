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
    // USER ROUTES
    [HttpGet]
    [Route("@user")]
    [Authenticated]
    public ActionResult<AuthorizedUserResponseDTO> GGetUser()
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(authUser);
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
    
    [HttpPatch]
    [Route("@user/update")]
    [Authenticated]
    public ActionResult<AuthorizedUserResponseDTO> PUpdateUser([FromBody] UserUpdateRequestDTO data)
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(service.UpdateUser(authUser.Id, data));
    }
    
    
    // ADMIN ROUTES
    [HttpPost]
    [Route("@admin/signup")]
    [Rolepolicy("Admin")]
    public ActionResult<AuthorizedUserResponseDTO> PSignup([FromBody] UserSignupRequestDTO request)
    {
        return Ok(service.Signup(request));
    }
    
    [HttpGet]
    [Route("@admin/users")]
    [Rolepolicy("Admin")]
    public ActionResult<AuthorizedUserResponseDTO[]> GGetUsers()
    {
        return Ok(service.GetUsers());
    }
    
    [HttpPatch]
    [Route("@admin/user")]
    [Rolepolicy("Admin")]
    public ActionResult<AuthorizedUserResponseDTO> PUpdateUserByAdmin([FromBody] UserUpdateByAdminRequestDTO data)
    {
        return Ok(service.UpdateUserByAdmin(data));
    }
}