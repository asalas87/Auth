using Application.Common.Responses;
using Application.Security.Common.DTOS;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.GetAll;
public record GetUsersPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<UserDTO>>>;