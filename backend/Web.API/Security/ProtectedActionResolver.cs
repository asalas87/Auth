using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.API.Security;

public static class ProtectedActionResolver
{
    private static readonly HashSet<string> ProtectedActions = new(StringComparer.OrdinalIgnoreCase)
    {
        "register",
        "login",
        "forgot-password",
        "reset-password"
    };

    public static string Resolve(ActionExecutingContext context)
    {
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

    public static bool IsProtected(string actionName) => ProtectedActions.Contains(actionName);
}
