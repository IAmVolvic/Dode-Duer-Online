using System.Security.Claims;
using DataAccess.Models;

namespace Service.Security;

public interface IJWTManager
{
    public string CreateJWT(User user);
    public ClaimsPrincipal IsJWTValid(string token);
}