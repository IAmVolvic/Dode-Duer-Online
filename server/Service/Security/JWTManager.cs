using System.Security.Claims;
using System.Text;
using DataAccess.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Service.Security;

public class JWTManager : IJWTManager
{
    
    private readonly string jwtToken = Environment.GetEnvironmentVariable("JWT_TOKEN")!;
    private readonly string jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
    private readonly string jtwAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
    
    public string CreateJWT(User user)
    {
        // Get the secret key for signing the token
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var tokenDescritor = new SecurityTokenDescriptor
        {
            Issuer = jwtIssuer,
            Audience = jtwAudience,
            Claims = new Dictionary<string, object>
            {
                ["UUID"] = user.Id,
                [JwtRegisteredClaimNames.Name] = user.Name,
                [JwtRegisteredClaimNames.Email] = user.Email
            },
            IssuedAt = null,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };
        
        var handler = new JsonWebTokenHandler();
        handler.SetDefaultTimesOnTokenCreation = false;

        return handler.CreateToken(tokenDescritor);
    }
    
    
    public ClaimsPrincipal IsJWTValid(string token)
    {
        var securityKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken));
        var tokenHandler = new JsonWebTokenHandler();

      
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jtwAudience,
            ValidateLifetime = true,
            IssuerSigningKey = securityKey,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true
        };
        
        var result = tokenHandler.ValidateTokenAsync(token, validationParameters).Result;
        
        if (result.IsValid)
        {
            return new ClaimsPrincipal(result.ClaimsIdentity);
        }

        return null;
    }
}