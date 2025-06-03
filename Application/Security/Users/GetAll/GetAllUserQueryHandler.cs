using Domain.Primitives;
using ErrorOr;
using MediatR;
using Domain.Security.Interfaces;
using Application.Security.Common.DTOS;
using Application.Common.Responses;

namespace Application.Security.Users.GetAll
{
    public sealed class GetUsersPaginatedQueryHandler : IRequestHandler<GetUsersPaginatedQuery, ErrorOr<PaginatedResult<UserDTO>>>
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
            }).ToList();

            return new PaginatedResult<UserDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
