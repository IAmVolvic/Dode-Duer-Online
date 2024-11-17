using API.Exceptions;
using DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
        try
        {
            var response = service.Signup(request);
            return Ok(response);
        }
        catch (ErrorExcep ex)
        {
            var errorResponse = new ErrorResponseDTO();
            errorResponse.AddError(ex.Source, ex.Description);
            return BadRequest(errorResponse);
        }
    }
    
    
    [HttpPost]
    [Route("login")]
    public ActionResult<UserResponseDTO> PLogin([FromBody] UserLoginRequestDTO data)
    {
        try
        {
            var response = service.Login(data);
            return Ok(response);
        }
        catch (ErrorExcep ex)
        {
            var errorResponse = new ErrorResponseDTO();
            errorResponse.AddError(ex.Source, ex.Description);
            return BadRequest(errorResponse);
        }
    }
}