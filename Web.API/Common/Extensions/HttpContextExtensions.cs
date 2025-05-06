using System.Security.Claims;

namespace Web.API.Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserIdOrThrow(this HttpContext context)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdClaim, out var userId)
                ? userId
                : throw new UnauthorizedAccessException("User ID not found or invalid.");
        }
    }
}
