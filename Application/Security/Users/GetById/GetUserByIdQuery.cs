using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.GetById;
public record GetUserByIdQuery(Guid Id) : IRequest<ErrorOr<User>>;