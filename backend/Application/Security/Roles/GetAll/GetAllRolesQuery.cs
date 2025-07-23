using Application.Security.Common.DTOS;
using ErrorOr;
using MediatR;

namespace Application.Security.Roles.GetAll;
public class GetAllRolesQuery : IRequest<ErrorOr<List<RoleDTO>>>;
