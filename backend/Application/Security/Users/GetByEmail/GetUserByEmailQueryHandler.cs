using ErrorOr;
using MediatR;
using Domain.Security.Entities;
using Application.Interfaces;
using Domain.Secutiry.Interfaces;

namespace Application.Security.Users.Create
{
    public sealed class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public GetUserByEmailQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<ErrorOr<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByEmailAsync(request.Email) is not User user)
                return Error.NotFound("User.NotFound", "The user with the provide email was not found.");

            if (!_passwordHasher.VerifyPassword(request.Password, user!.Password))
                return Error.Unauthorized("User.EmailOrPass", "Email o password incorrecto");

            return user;
        }
    }
}
