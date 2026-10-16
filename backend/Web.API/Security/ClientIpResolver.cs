namespace Web.API.Security;

public static class ClientIpResolver
{
    private const string CloudflareHeader = "CF-Connecting-IP";
    private const string UnknownIp = "unknown";

    public static string Resolve(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue(CloudflareHeader, out var cfIp))
        {
            var ip = cfIp.ToString();
            if (!string.IsNullOrWhiteSpace(ip))
                return ip;
        }

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? UnknownIp;
    }
}
