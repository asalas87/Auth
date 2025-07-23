using Domain.Primitives;
using ErrorOr;
using MediatR;
using Application.Security.Common.DTOS;
using Application.Common.Responses;
using Domain.Secutiry.Interfaces;

namespace Application.Security.Users.GetAll
{
    public sealed class GetUsersPaginatedQueryHandler : IRequestHandler<GetUsersPaginatedQuery, ErrorOr<PaginatedResult<UserDTO>>>, IRequestHandler<GetAllUsersQuery, ErrorOr<List<UserDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public GetUsersPaginatedQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<PaginatedResult<UserDTO>>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedAsync(request.Page, request.PageSize, request.Filter);

            var items = users.Select(u => new UserDTO
            {
                Id = u.Id.Value,
                Name = u.Name,
                Email = u.Email.Value,
                Role = u.Role.Name ?? string.Empty,
                RoleId = u.Role.Id
            }).ToList();

            return new PaginatedResult<UserDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<ErrorOr<List<UserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAll();

            var items = users.Select(u => new UserDTO
            {
                Id = u.Id.Value,
                Name = u.Name,
                Email = u.Email.Value,
            }).ToList();

            return items;
        }
    }
}
