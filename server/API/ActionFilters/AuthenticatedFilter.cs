using API.Attributes;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Services.Interfaces;

namespace API.ActionFilters;

public class AuthenticatedFilter : IActionFilter
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthenticatedFilter> _logger;
    
    public AuthenticatedFilter(IUserService userService, ILogger<AuthenticatedFilter> logger)
    {
        _userService = userService;
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
        var cookies = GetCookie(context);
        _userService.IsUserAuthenticated(cookies["Authentication"]!);
    }
    
    
    private void HasAuthorization(ActionExecutingContext context)
    {
        var cookies = GetCookie(context);
        IsAuthenticated(context);
        
        var rolepolicyAttribute =
            context.ActionDescriptor.EndpointMetadata.OfType<RolepolicyAttribute>().FirstOrDefault();

        _userService.IsUserAuthorized(rolepolicyAttribute!.Roles, cookies["Authentication"]!);
    }
    
    
    private IRequestCookieCollection GetCookie(ActionExecutingContext context)
    {
        var cookies = context.HttpContext.Request.Cookies;
        if (!cookies.TryGetValue("Authentication", out var accessToken) || string.IsNullOrEmpty(accessToken))
        {
            throw new ErrorException("Authentication", "Missing authentication token.");
        }
        return cookies;
    }
}