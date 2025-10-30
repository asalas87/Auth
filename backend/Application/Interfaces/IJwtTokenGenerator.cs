using System.Security.Claims;

namespace Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(IEnumerable<Claim> claims, DateTime expires);
}
