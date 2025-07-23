using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Security.Roles.GetById;
public record GetRoleByIdQuery(int Id) : IRequest<ErrorOr<Role>>;
