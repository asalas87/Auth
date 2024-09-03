using Domain.Secutiry.Entities;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.Create;
public record GetUserByEmailQuery(
    Email Email,
    string Password) : IRequest<ErrorOr<User>>;