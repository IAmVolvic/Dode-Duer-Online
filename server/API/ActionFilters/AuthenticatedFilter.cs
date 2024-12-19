using API.Attributes;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Service.Services.Interfaces;

namespace API.ActionFilters;

public class AuthenticatedFilter : IActionFilter
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthenticatedFilter> _logger;
    
    public AuthenticatedFilter(IAuthService authService, ILogger<AuthenticatedFilter> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var hasAuthenticatedAttribute = 
            context.ActionDescriptor.EndpointMetadata.Any(metadata => metadata is AuthenticatedAttribute);
        var hasRolepolicyAttribute =
            context.ActionDescriptor.EndpointMetadata.Any(metadata => metadata is RolepolicyAttribute);
        
        if (hasAuthenticatedAttribute)
        {
            IsAuthenticated(context);
        }
        
        if (hasRolepolicyAttribute)
        {
            HasAuthorization(context);
        }
    }
    
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // You can add logging or other post-execution logic here if needed.
    }

    private void IsAuthenticated(ActionExecutingContext context)
    {
        var accessToken = GetBearerToken(context);
        _authService.IsUserAuthenticated(accessToken);
        context.HttpContext.Items["AuthenticatedUser"] = _authService.GetAuthorizedUser(accessToken);
    }

    private void HasAuthorization(ActionExecutingContext context)
    {
        var accessToken = GetBearerToken(context);
        IsAuthenticated(context);
        
        var rolepolicyAttribute =
            context.ActionDescriptor.EndpointMetadata.OfType<RolepolicyAttribute>().FirstOrDefault();

        _authService.IsUserAuthorized(rolepolicyAttribute!.Roles, accessToken);
    }

    private string GetBearerToken(ActionExecutingContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            throw new ErrorException("Authorization", "Missing or invalid Bearer token.");
        }

        // Extract the token by removing "Bearer " prefix.
        return authorizationHeader.Substring("Bearer ".Length).Trim();
    }
}