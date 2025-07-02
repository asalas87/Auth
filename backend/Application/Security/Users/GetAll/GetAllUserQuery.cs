using Application.Security.Common.DTOS;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.GetAll;
public record GetAllUsersQuery() : IRequest<ErrorOr<List<UserDTO>>>;
