using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Secutiry.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Security.Users.GetById
{
    public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<ErrorOr<User>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByIdAsync(new UserId(query.Id)) is not User user)
            {
                return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
            }

            return user;
        }
    }
}
