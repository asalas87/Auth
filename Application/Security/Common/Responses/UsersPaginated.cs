using Domain.Secutiry.Entities;

namespace Application.Security.Common.Responses
{
    public record UsersPaginated(List<User> Users, int Count);
}
