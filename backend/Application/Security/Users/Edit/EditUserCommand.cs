using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Edit;
public record EditUserCommand(
    Guid Id,
    string Name,
    string Email,
    int RoleId,
    Guid CompanyId
) : IRequest<ErrorOr<UserId>>;

