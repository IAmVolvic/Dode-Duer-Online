using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Service.Security;

public class JWTManager : IJWTManager
{
    
    private readonly string jt = "UNKNWON";
    private readonly string ji = "UNKNWON";
    private readonly string ja = "UNKNWON";
    
    public string CreateJWT(User user)
    {
        // Get the secret key for signing the token
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jt.PadRight(256 / 8, '\0')));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Define the claims for the token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var claimsDictionary = claims.ToDictionary(c => c.Type, c => (object)c.Value);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = ji,
            Audience = ja,
            Claims = claimsDictionary,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.InboundClaimTypeMap.Clear();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Return the generated JWT as a string
        return tokenHandler.WriteToken(token);
    }

    
    
    public ClaimsPrincipal IsJWTValid(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jt.PadRight(256 / 8, '\0')));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            Console.WriteLine(token);
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var tokenAudience = jwtToken.Audiences.FirstOrDefault(); // Get the 'aud' claim from the token
            Console.WriteLine($"Audience in token: {tokenAudience}");
            Console.WriteLine($"Token: {jwtToken}");
            
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = ji,
                ValidateAudience = true,
                ValidAudience = ja,
                ValidateLifetime = true,
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true
            };
            
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                if (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }
            }
            
        } catch (Exception ex) {
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }

        return null;
    }
}