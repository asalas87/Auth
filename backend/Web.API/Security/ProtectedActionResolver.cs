using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Web.API.Security;

public class ProtectedActionResolver
{
    private readonly SecurityRateLimitingOptions _options;

    public ProtectedActionResolver(IOptions<SecurityRateLimitingOptions> options)
    {
        _options = options.Value;
    }

    public string Resolve(ActionExecutingContext context)
    {
        var actionDescriptor = context.ActionDescriptor;

        var attribute = actionDescriptor.EndpointMetadata
            .OfType<ProtectedActionAttribute>()
            .FirstOrDefault();

        if (attribute != null)
        {
            return attribute.ActionName.ToLowerInvariant();
        }

        var action = context.ActionDescriptor.RouteValues["action"];
        return action?.ToLowerInvariant() switch
        {
            "register" => "register",
            "login" => "login",
            "forgotpassword" => "forgot-password",
            "resetpassword" => "reset-password",
            _ => action?.ToLowerInvariant() ?? "unknown"
        };
    }

    public bool IsProtected(string actionName)
    {
        return _options.Actions.ContainsKey(actionName);
    }

    public SecurityActionOptions? GetOptions(string actionName)
    {
        if (_options.Actions.TryGetValue(actionName, out var options))
        {
            return options;
        }
        return null;
    }
}
