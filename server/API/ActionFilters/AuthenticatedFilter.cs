using API.Attributes;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Services.Interfaces;

namespace API.ActionFilters;

public class AuthenticatedFilter(IUserService userService) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check if the action has the AuthenticatedAttribute
        var hasAuthenticatedAttribute = context.ActionDescriptor
            .EndpointMetadata
            .Any(metadata => metadata is AuthenticatedAttribute);

        // If the action has the AuthenticatedAttribute, do authentication check
        if (hasAuthenticatedAttribute)
        {
            var cookies = context.HttpContext.Request.Cookies;
            if (!cookies.TryGetValue("Authentication", out var accessToken) || string.IsNullOrEmpty(accessToken))
            {
                throw new ErrorException("Authentication", "Authentication token is missing or invalid.");
            }
           
            userService.IsUserAuthenticated(cookies["Authentication"]);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // You can add logging or other post-execution logic here if needed.
    }
}