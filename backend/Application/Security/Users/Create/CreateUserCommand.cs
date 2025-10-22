using Domain.Partners.Entities;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Create
{
    public record CreateUserCommand(
        string Name,
        Email Email,
        CompanyId CompanyId,
        int RoleId) : IRequest<ErrorOr<Guid>>;
}
