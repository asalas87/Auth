using Domain.Primitives;
using ErrorOr;
using MediatR;
using AutoMapper;
using Domain.Secutiry.Entities;
using Domain.Secutiry.Interfaces;
using Application.Interfaces;

namespace Application.Security.Users.Create
{
    public sealed class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ErrorOr<User>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public GetUserByEmailQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<ErrorOr<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByEmailAsync(request.Email) is not User user)
                return Error.Failure("User.NotFound", "The user with the provide email was not found.");

            if (!_passwordHasher.VerifyPassword(request.Password, user!.Password))
                return Error.Failure("User.EmailOrPass", "Email o password incorrecto");

            return user;
        }
    }
}
