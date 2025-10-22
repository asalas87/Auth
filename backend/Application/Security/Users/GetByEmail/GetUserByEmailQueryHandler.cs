using ErrorOr;
using MediatR;
using Domain.Security.Entities;
using Application.Interfaces;
using Domain.Security.Interfaces;
using Application.Security.Users.Create;

namespace Application.Security.Users.GetByEmail
{
    public sealed class GetUserByEmailQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<GetUserByEmailQuery, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));

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
