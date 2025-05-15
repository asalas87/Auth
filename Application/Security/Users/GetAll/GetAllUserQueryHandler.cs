using Domain.Primitives;
using ErrorOr;
using MediatR;
using Domain.Secutiry.Interfaces;
using Application.Interfaces;
using Application.Security.Common.DTOS;
using Application.Common.Responses;

namespace Application.Security.Users.GetAll
{
    public sealed class GetUsersPaginatedQueryHandler : IRequestHandler<GetUsersPaginatedQuery, ErrorOr<PaginatedResult<UserDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public GetUsersPaginatedQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<ErrorOr<PaginatedResult<UserDTO>>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedAsync(request.Page, request.PageSize, request.Filter);

            var items = users.Select(u => new UserDTO
            {
                Id = u.Id.Value.ToString(),
                Name = u.Name,
                Email = u.Email.Value,
            }).ToList();

            return new PaginatedResult<UserDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
